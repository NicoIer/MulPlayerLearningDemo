﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class GameStateMachine : IController
    {
        private Dictionary<Type, GameState> _states = new();
        public GameManager Owner { get; private set; }
        public GameState CurrentState { get; private set; }
        public Action<GameState, GameState> OnStateChange;

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
            OnStateChange?.Invoke(CurrentState, CurrentState);
            Debug.Log($"{CurrentState?.GetType()} to {nextState?.GetType()}");
            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState?.Enter();
            
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}