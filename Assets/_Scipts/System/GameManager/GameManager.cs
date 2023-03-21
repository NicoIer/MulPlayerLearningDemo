using System;
using Nico;
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
            StartGame();
        }

        private void Update()
        {
            if (Player.Player.Instance.input.Player.Pause.WasPerformedThisFrame())
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