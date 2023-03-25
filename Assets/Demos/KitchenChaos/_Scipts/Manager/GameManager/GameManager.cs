using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Kitchen.UI;
using Nico.Design;
using Nico.Network;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen
{
    public class GameManager : NetSingleton<GameManager>
    {
        [field: SerializeField] public GameSetting setting { get; private set; }
        public event Action<int> OnCountDownChange;
        public event Action<float> OnLeftTimeChange;
        public event Action OnLocalPlayerReady;
        public GameStateMachine stateMachine { get; private set; }
        public GameState CurrentState => stateMachine.CurrentState;
        public bool localPlayerReady { get; private set; } = false;

        private HashSet<ulong> _playerReadySet = new HashSet<ulong>();

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
            stateMachine.Add(new WaitingToStartState());
            stateMachine.Add(new ReadyToStartState());
            stateMachine.Add(new PlayingState());
            stateMachine.Add(new PausedState());
            stateMachine.Add(new GameOverState());
        }

        private void Start()
        {
            PlayerInput.Instance.OnInteractPerform += OnPerformInteract;
            stateMachine.Change<WaitingToStartState>();
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
            // stateMachine.Change<ReadyToStartState>();
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
            Debug.Log("Pause Game");
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

        #endregion

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
        internal void ChangeStateClientRpc(GameStateEnum stateEnum)
        {
            stateMachine.Change(stateEnum);
        }

        [ClientRpc]
        internal void OnCountDownChangeClientRpc(int num)
        {
            OnCountDownChange?.Invoke(num);
        }
        
        [ClientRpc]
        internal void OnLeftTimeChangeClientRpc(float timer)
        {
            OnLeftTimeChange?.Invoke(timer);
        }
    }
}