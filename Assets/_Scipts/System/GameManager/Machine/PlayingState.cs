using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class PlayingState : GameState
    {
        public event Action OnStartPlaying;

        public override void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                stateMachine.Change<GameOverState>();
            }
        }

        public override void Enter()
        {
            OnStartPlaying?.Invoke();
        }
    }
}