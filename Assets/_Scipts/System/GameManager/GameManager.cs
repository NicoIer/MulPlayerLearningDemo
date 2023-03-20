using System;
using Nico.DesignPattern;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameSetting setting;
        public int readyCountDown = 3;
        public event Action<int> OnCountDownChange;
        private GameStateMachine _stateMachine;

        //TODO 这里的stateMachine就不能让外部访问
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
            if (Keyboard.current.pKey.wasPressedThisFrame)
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