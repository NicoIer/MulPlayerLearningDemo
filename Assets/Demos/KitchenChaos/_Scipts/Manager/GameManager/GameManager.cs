using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Kitchen.Config;
using Kitchen.UI;
using Nico.Design;
using Nico.Network;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kitchen
{
    public class GameManager : GlobalNetSingleton<GameManager>
    {
        [field: SerializeField] public GameSetting setting { get; private set; }

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

        protected override void Awake()
        {
            base.Awake();
            if (stateMachine == null)
            {
                _Init_StateMachine();
            }
        }

        private void _Init_StateMachine()
        {
            stateMachine = new GameStateMachine(this);
            stateMachine.Add(new LobbyState());
            stateMachine.Add(new WaitingToStartState());
            stateMachine.Add(new ReadyToStartState());
            stateMachine.Add(new PlayingState());
            stateMachine.Add(new PausedState());
            stateMachine.Add(new GameOverState());
        }

        private void Start()
        {
            PlayerInput.Instance.OnInteractPerform += OnPerformInteract;
            stateMachine.Change<LobbyState>();
        }


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
            NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;
            NetworkManager.Singleton.StartHost();
        }

        public void StartClient()
        {
            //TODO 值得研究
            OnConnecting?.Invoke();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnConnectingFailed;
            NetworkManager.Singleton.StartClient();
        }

        #endregion


        /// <summary>
        /// TODO 搞懂这个方法
        /// 这个方法是在客户端连接服务器的时候调用的
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

        public void EnterGame()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadSceneCompleted;
        }

        public void OnLoadSceneCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted,
            List<ulong> clientstimedout)
        {
            Debug.Log($"{scenename}加载完成");
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
    }
}