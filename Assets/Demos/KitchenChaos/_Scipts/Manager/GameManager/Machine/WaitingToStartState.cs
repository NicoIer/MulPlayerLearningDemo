using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class WaitingToStartState : GameState
    {
        public static readonly GameStateEnum stateEnum = GameStateEnum.WaitingToStart;
        // public override void Update()
        // {
        //     if (Keyboard.current.spaceKey.wasPressedThisFrame)
        //     {
        //         stateMachine.Change<ReadyToStartState>();
        //         return;
        //     }
        // }
    }
}