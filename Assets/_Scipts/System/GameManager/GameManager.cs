using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class GameManager : MonoBehaviour
    {
        public GameSetting setting;
        public int readyCountDown = 3;
        public event Action<int> OnCountDownChange;
        public GameStateMachine stateMachine { get; private set; }

        public static GameManager Instance { get; private set; }

        protected void Awake()
        {
            Instance = this;
            _Init_StateMachine();
        }

        private void _Init_StateMachine()
        {
            stateMachine = new GameStateMachine(this);
            stateMachine.Add(new WaitingToStartState());
            var readyToStartState = new ReadyToStartState();
            readyToStartState.onCountDownChange += (num) => { OnCountDownChange?.Invoke(num); };
            stateMachine.Add(readyToStartState);
            stateMachine.Add(new PlayingState());
            stateMachine.Add(new PausedState());
            stateMachine.Add(new GameOverState());
            
        }
        private void OnDestroy()
        {
            Instance = null;
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