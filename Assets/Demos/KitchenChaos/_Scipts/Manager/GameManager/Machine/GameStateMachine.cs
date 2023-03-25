using System;
using System.Collections.Generic;
using Nico.ECC;
using UnityEngine;

namespace Kitchen
{
    public class GameStateMachine : IController<GameManager>
    {
        private Dictionary<Type, GameState> _states = new();


        public GameState CurrentState { get; private set; }
        public Action<GameState, GameState> onStateChange;

        public GameManager Owner { get; protected set; }

        public GameStateMachine(GameManager manager)
        {
            Owner = manager;
        }

        public void Add(GameState state)
        {
            state.SetMachine(this);
            _states.Add(state.GetType(), state); //GetType()返回当前实例的运行时类型
        }

        public void Change<T>() where T : GameState
        {
            var nextState = _states[typeof(T)];
            onStateChange?.Invoke(CurrentState, nextState);
            Debug.Log($"{CurrentState?.GetType()} to {nextState?.GetType()}");
            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState?.Enter();
        }

        public void Change(GameStateEnum stateEnum)
        {
            //找到GameStateEnum对应的类型
            switch (stateEnum)
            {
                case GameStateEnum.WaitingToStart:
                    Change<WaitingToStartState>();
                    break;
                case GameStateEnum.ReadyToStart:
                    Change<ReadyToStartState>();
                    break;
                case GameStateEnum.Playing:
                    Change<PlayingState>();
                    break;
                case GameStateEnum.Paused:
                    Change<PausedState>();
                    break;
                case GameStateEnum.GameOver:
                    Change<GameOverState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEnum), stateEnum, null);
            }
        }


        public void Update()
        {
            CurrentState?.Update();
        }
    }
}