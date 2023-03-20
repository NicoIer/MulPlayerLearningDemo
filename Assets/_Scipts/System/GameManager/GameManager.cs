using System;
using UnityEngine;

namespace Kitchen
{
    public class GameManager : MonoBehaviour
    {
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
            stateMachine.Change<WaitingToStartState>();
        }

        private void Update()
        {
            stateMachine.Update();
        }
        

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