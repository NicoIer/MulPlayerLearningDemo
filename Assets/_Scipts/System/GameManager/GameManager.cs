using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class GameManager : MonoBehaviour
    {
        public int readyCountDown = 3;
        public event Action OnGameReadyToStart;
        public event Action<int> OnCountDownChange;
        public event Action OnStartPlaying;
        public GameStateMachine stateMachine { get; private set; }

        public static GameManager Instance { get; private set; }

        protected void Awake()
        {
            Instance = this;
            Debug.Log("Welcome to Kitchen!");
            stateMachine = new GameStateMachine(this);
            stateMachine.Add(new WaitingToStartState());
            var readyToStartState = new ReadyToStartState();
            readyToStartState.onCountDownChange += (num) => { OnCountDownChange?.Invoke(num); };
            readyToStartState.OnGameReadyToStart += () => { OnGameReadyToStart?.Invoke(); };
            stateMachine.Add(readyToStartState);
            var playingState = new PlayingState();
            playingState.OnStartPlaying += () => { OnStartPlaying?.Invoke(); };
            stateMachine.Add(playingState);
            stateMachine.Add(new PausedState());
            stateMachine.Add(new GameOverState());
            stateMachine.Change<WaitingToStartState>();
            // stateMachine.OnStateChange += _StateMachine_OnStateChange;
        }

        private void Update()
        {
            stateMachine.Update();
        }

        // private void _StateMachine_OnStateChange(GameState old, GameState next)
        // {
        // }

        public bool IsPlaying()
        {
            return stateMachine.CurrentState is PlayingState;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}