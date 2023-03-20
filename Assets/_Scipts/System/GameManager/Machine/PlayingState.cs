using System;

namespace Kitchen
{
    public class PlayingState: GameState
    {
        public event Action OnStartPlaying;
        public override void Enter()
        {
            OnStartPlaying?.Invoke();
        }
    }
}