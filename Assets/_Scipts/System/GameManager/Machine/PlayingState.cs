using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class PlayingState : GameState
    {
        private float gameDuration => owner.setting.gameDurationSetting;
        private float _currentTime;
        public event Action<float> onLeftTimeChange;

        public override void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                stateMachine.Change<GameOverState>();
            }
        }

        public override void Enter()
        {
            //开启计时器
            _StartPlaying().Forget();
        }

        private CancellationTokenSource _playingTokenSource;

        private async UniTask _StartPlaying()
        {
            _playingTokenSource = new CancellationTokenSource();
            _currentTime = gameDuration;
            while (_playingTokenSource.IsCancellationRequested == false)
            {
                if (_currentTime <= 0)
                    break;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _playingTokenSource.Token);
                _currentTime -= 0.5f;
                onLeftTimeChange?.Invoke(_currentTime);
            }

            stateMachine.Change<GameOverState>();
        }

        public override void Exit()
        {
            _playingTokenSource?.Cancel();
        }
    }
}