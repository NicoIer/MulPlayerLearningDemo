using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class PausedState : GameState
    {
        public static readonly GameStateEnum stateEnum = GameStateEnum.Paused;
        public override void Enter()
        {
            Time.timeScale = 0;
        }
        public override void Exit()
        {
            Time.timeScale = 1;
        }
    }
}