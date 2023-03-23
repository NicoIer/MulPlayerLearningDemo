using System;
using Cysharp.Threading.Tasks;
using Kitchen.UI;
using Nico.Network.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private GameSetting _setting;

        public GameSetting setting
        {
            get
            {
                if (_setting == null)
                {
                    //TODO 改成从配置文件读取
                    _setting = new GameSetting();
                    setting.gameDurationSetting = 60;
                }

                return _setting;
            }
        }

        public int readyCountDown = 3;
        public event Action<int> OnCountDownChange;
        private GameStateMachine _stateMachine;

        public GameStateMachine stateMachine
        {
            get
            {
                //这里不是线程安全的
                if (_stateMachine == null)
                    _Init_StateMachine();
                return _stateMachine;
            }
        }

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
            _stateMachine = new GameStateMachine(this);
            _stateMachine.Add(new WaitingToStartState());
            var readyToStartState = new ReadyToStartState();
            readyToStartState.onCountDownChange += (num) => { OnCountDownChange?.Invoke(num); };
            _stateMachine.Add(readyToStartState);
            _stateMachine.Add(new PlayingState());
            _stateMachine.Add(new PausedState());
            _stateMachine.Add(new GameOverState());
        }

        private void Start()
        {
            // PlayerInput.Instance.OnInteractPerform += OnPerformInteract;
            // _stateMachine.Change<WaitingToStartState>();
            //TODO 测试时 等待选择连接方式后 再 开始游戏
            _stateMachine.Change<WaitingToStartState>();
            UniTask.WaitUntil(() => TestingUI.testingBool).ContinueWith(StartGame).Forget();
        }


        private void OnPerformInteract()
        {
            if (stateMachine.CurrentState is WaitingToStartState)
            {
                StartGame();
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

            stateMachine.Update();
        }

        public void StartGame()
        {
            stateMachine.Change<ReadyToStartState>();
        }

        public void PauseGame()
        {
            Debug.Log($"PauseGame: from {stateMachine.CurrentState.GetType()}");
            if (stateMachine.CurrentState is PausedState)
            {
                stateMachine.Change<PlayingState>();
            }
            else if (stateMachine.CurrentState is PlayingState)
            {
                stateMachine.Change<PausedState>();
            }
        }

        public void ExitGame()
        {
            stateMachine.Change<WaitingToStartState>(); //切换
        }

        public bool IsPlaying()
        {
            return stateMachine.CurrentState is PlayingState;
        }
    }
}