using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Model;
using Nico.MVC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class PlayingState : GameState
    {
        private float gameDuration => owner.setting.gameDurationSetting;
        private float _currentTime;
        public event Action<float> OnLeftTimeChange;
        private CancellationTokenSource _playingTokenSource;
        private bool _firstEnter = true;
        

        public override void Enter()
        {
            if (_firstEnter)
            {
                _firstEnter = false;
                _currentTime = gameDuration;
                ModelManager.Get<CompletedOrderModel>().orderCount = 0;
                //开启计时器
            }
            _StartPlaying().Forget();
        }


        private async UniTask _StartPlaying()
        {
            _playingTokenSource = new CancellationTokenSource();
            while (_playingTokenSource.IsCancellationRequested == false)
            {
                if (_currentTime <= 0)
                    break;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _playingTokenSource.Token);
                _currentTime -= 0.5f;
                OnLeftTimeChange?.Invoke(_currentTime);
            }

            stateMachine.Change<GameOverState>();
        }

        public override void Exit()
        {
            _playingTokenSource?.Cancel();
        }
    }
}