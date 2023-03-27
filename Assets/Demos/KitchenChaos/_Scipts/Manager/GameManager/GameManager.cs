using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Kitchen.Config;
using Kitchen.Scene;
using Nico.Network;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kitchen
{
    public class GameManager : GlobalNetSingleton<GameManager>
    {
        [field: SerializeField] public GameSetting setting { get; private set; }

        public NetworkList<PlayerConfig> playerConfigs { get; private set; }
        [field: SerializeField] public List<Color> playerColors { get; private set; } = new List<Color>();
        private string _playerName;

        public string playerName
        {
            get { return _playerName; }
            set
            {
                _playerName = value;
                PlayerPrefs.SetString(nameof(_playerName), _playerName);
            }
        }

        #region Events

        public event Action<int> OnCountDownChange;
        public event Action<float> OnLeftTimeChange;
        public event Action OnLocalPlayerReady;
        public event Action OnConnecting;
        public event Action<ulong> OnConnectingFailed;

        #endregion

        public GameStateMachine stateMachine { get; private set; }
        public GameState CurrentState => stateMachine.CurrentState;
        public bool localPlayerReady { get; private set; } = false;

        private HashSet<ulong> _playerReadySet = new HashSet<ulong>();
        [SerializeField] private GameObject playerPrefab;

        #region Awake Start

        protected override void Awake()
        {
            base.Awake();
            _playerName = PlayerPrefs.GetString(nameof(_playerName), "Nico");
            if (stateMachine == null)
            {
                stateMachine = new GameStateMachine(this);
                stateMachine.Add(new LobbyState());
                stateMachine.Add(new WaitingToStartState());
                stateMachine.Add(new ReadyToStartState());
                stateMachine.Add(new PlayingState());
                stateMachine.Add(new PausedState());
                stateMachine.Add(new GameOverState());
            }

            if (playerConfigs == null)
            {
                playerConfigs = new();
            }
        }

        private void Start()
        {
            PlayerInput.Instance.OnInteractPerform += OnPerformInteract;
            stateMachine.Change<LobbyState>();
        }

        #endregion


        private void OnPerformInteract()
        {
            //在等待开始状态下，按下交互键，进入准备完成状态
            if (stateMachine.CurrentState is WaitingToStartState)
            {
                localPlayerReady = true;
                OnLocalPlayerReady?.Invoke();
                SetPlayerReadyServerRpc();
                PlayerInput.Instance.OnInteractPerform -= OnPerformInteract;
            }
        }

        private void Update()
        {
            if (PlayerInput.Instance.Player.Pause.WasPerformedThisFrame())
            {
                PauseGame();
                return;
            }

            if (IsServer) //只有服务器做状态的更新
            {
                stateMachine.Update();
            }
        }

        #region 游戏 状态

        public void EnterGame()
        {
            //TODO 使用Rpc 替代 NetworkManager.Singleton.SceneManager.LoadScene
            SceneLoader.LoadNet(SceneName.GameScene);
            // NetworkManager.Singleton.SceneManager.LoadScene(SceneName.GameScene, LoadSceneMode.Single);
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadSceneCompleted;
        }



        public void OnLoadSceneCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted,
            List<ulong> clientstimedout)
        {
            Debug.Log($"{scenename}加载完成,isServer:{IsServer}");
            if (!IsServer)
                return;
            if (scenename != SceneName.GameScene)
                return;
            ChangeStateClientRpc(WaitingToStartState.stateEnum); //通知所有客户端切换状态
            //同时 由服务端 生成玩家
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                var playerObj = Instantiate(playerPrefab);
                playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            }

            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadSceneCompleted;
        }


        public void StartGame()
        {
            ChangeStateClientRpc(ReadyToStartState.stateEnum);
        }

        /// <summary>
        /// 暂停游戏 只有服务器才能暂停游戏
        /// </summary>
        public void PauseGame()
        {
            //只有服务器才能暂停游戏
            if (!IsServer)
            {
                return;
            }

            if (stateMachine.CurrentState is PausedState)
            {
                // stateMachine.Change<PlayingState>();
                ChangeStateClientRpc(PlayingState.stateEnum);
            }
            else if (stateMachine.CurrentState is PlayingState)
            {
                // stateMachine.Change<PausedState>();
                ChangeStateClientRpc(PausedState.stateEnum);
            }
        }

        public void ExitGame()
        {
            ChangeStateClientRpc(WaitingToStartState.stateEnum);
            // stateMachine.Change<WaitingToStartState>(); //切换
        }

        public bool IsPlaying()
        {
            return stateMachine.CurrentState is PlayingState;
        }

        [ClientRpc]
        internal void ChangeStateClientRpc(GameStateEnum stateEnum)
        {
            stateMachine.Change(stateEnum);
        }

        [ClientRpc]
        internal void OnCountDownChangeClientRpc(int num)
        {
            OnCountDownChange?.Invoke(num);
        }

        #endregion

        #region 同步 Rpc

        [ServerRpc(RequireOwnership = false)]
        internal void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            _playerReadySet.Add(serverRpcParams.Receive.SenderClientId);
            bool flag = NetworkManager.Singleton.ConnectedClientsIds.All(clientId =>
                _playerReadySet.Contains(clientId));

            if (flag)
            {
                StartGame();
            }
        }


        [ClientRpc]
        internal void OnLeftTimeChangeClientRpc(float timer)
        {
            OnLeftTimeChange?.Invoke(timer);
        }

        #endregion


        #region 启动服务器 客户端

        public void StartHost()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback; // 是否允许连接 事件
            NetworkManager.Singleton.OnClientConnectedCallback += Host_OnClientConnectedCallback; // 客户端连接事件 回调
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisConnectedCallback; // 客户端断开连接事件 回调
            NetworkManager.Singleton.StartHost();
        }

        private void OnClientDisConnectedCallback(ulong clientId)
        {
            Debug.Log($"客户端{clientId}断开连接");
            int targetIdx = -1;
            for (int i = 0; i < playerConfigs.Count; i++)
            {
                if (clientId == playerConfigs[i].clientId)
                {
                    targetIdx = i;
                    break;
                }
            }

            if (targetIdx != -1)
            {
                playerConfigs.RemoveAt(targetIdx);
            }
        }


        public void StartClient()
        {
            //TODO 值得研究
            OnConnecting?.Invoke();
            NetworkManager.Singleton.OnClientConnectedCallback += Client_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnSelfDisconnectCallback;
            Debug.Log("NetworkManager 开启客户端 准备连接");
            NetworkManager.Singleton.StartClient();
        }


        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerNameServerRpc(string name, ServerRpcParams serverRpcParams = default)
        {
            Debug.Log($"客户端{serverRpcParams.Receive.SenderClientId}设置名字为{name}");
            var senderID = serverRpcParams.Receive.SenderClientId;
            for (int i = 0; i != playerConfigs.Count; ++i)
            {
                var playerConfig = playerConfigs[i];
                if (playerConfig.clientId == senderID)
                {
                    playerConfig.playerName = name;
                    playerConfigs[i] = playerConfig; //结构体赋值 不会改变引用
                    break;
                }
            }
        }


        public void OnSelfDisconnectCallback(ulong clientId)
        {
            OnConnectingFailed?.Invoke(clientId);
        }

        #endregion


        #region 回调事件

        private void Host_OnClientConnectedCallback(ulong clientId)
        {
            Debug.Log($"{clientId} 连接成功");
            playerConfigs.Add(new PlayerConfig
            {
                clientId = clientId,
                colorId = 0
            });
            SetPlayerNameServerRpc(playerName);
            SetPlayerIDServerRpc(AuthenticationService.Instance.PlayerId);
        }

        private void Client_OnClientConnectedCallback(ulong clientId)
        {
            Debug.Log($"客户端连接成功,客户端id{clientId}");
            SetPlayerNameServerRpc(_playerName);
            SetPlayerIDServerRpc(AuthenticationService.Instance.PlayerId);
        }

        /// <summary>
        /// TODO 搞懂这个方法
        /// 这个方法是在客户端尝试连接服务器的时候调用的
        /// request代表客户端的请求
        /// response代表服务器的响应
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log("ConnectionApprovalCallback!");
            if (CurrentState is not LobbyState)
            {
                Debug.Log("拒绝连接");
                response.Approved = false;
                response.Reason = "Game Already Started";
                return;
            }

            if (NetworkManager.Singleton.ConnectedClientsIds.Count >= setting.maxPlayerCount)
            {
                Debug.Log("拒绝连接");
                response.Approved = false;
                response.Reason = "Player is full";
                return;
            }

            if (CurrentState is LobbyState)
            {
                Debug.Log("允许连接");
                response.Approved = true;
                response.CreatePlayerObject = false;
                return;
            }
        }

        #endregion

        public Color GetColor(int colorIdx)
        {
            return playerColors[colorIdx];
        }

        public PlayerConfig GetPlayerConfig(ulong clientId)
        {
            for (int i = 0; i != playerConfigs.Count; ++i)
            {
                var playerConfig = playerConfigs[i];
                if (clientId == playerConfig.clientId)
                {
                    var config = playerConfig;
                    config.spawnPointId = i;
                    return config;
                }
            }

            Debug.LogWarning("没有找到玩家配置");

            return default;
        }

        public PlayerConfig GetLocalPlayerConfig()
        {
            var id = NetworkManager.Singleton.LocalClientId;
            return GetPlayerConfig(id);
        }

        public void SePlayerColor(int colorIdx)
        {
            SetPlayerColorServerRpc(colorIdx);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
        {
            var senderID = serverRpcParams.Receive.SenderClientId;
            for (int i = 0; i != playerConfigs.Count; ++i)
            {
                var playerConfig = playerConfigs[i];
                if (playerConfig.clientId == senderID)
                {
                    playerConfig.colorId = colorId;
                    playerConfigs[i] = playerConfig; //结构体赋值 不会改变引用
                    break;
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerIDServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
        {
            var senderID = serverRpcParams.Receive.SenderClientId;
            var idx = GetConfigIdxByClientId(senderID);
            if (idx == -1)
            {
                Debug.LogError("没有找到玩家配置");
                return;
            }

            var playerConfig = playerConfigs[idx];
            if (playerConfig.clientId == senderID)
            {
                playerConfig.playerId = playerId;
                playerConfigs[idx] = playerConfig; //结构体赋值 不会改变引用
            }
        }

        private int GetConfigIdxByClientId(ulong clientId)
        {
            for (int i = 0; i != playerConfigs.Count; ++i)
            {
                var playerConfig = playerConfigs[i];
                if (playerConfig.clientId == clientId)
                {
                    return i;
                }
            }

            return -1;
        }

        public void KickPlayer(ulong clientId)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
            OnClientDisConnectedCallback(clientId);
        }
    }
}