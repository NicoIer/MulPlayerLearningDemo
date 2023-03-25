using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nico.MVC;

namespace Kitchen
{
    public class PlayingState : GameState
    {
        public static readonly GameStateEnum stateEnum = GameStateEnum.Playing;
        private float gameDuration => owner.setting.gameDurationSetting;
        private float _currentTime;
        private CancellationTokenSource _playingTokenSource;
        private bool _firstEnter = true;


        public override void Enter()
        {
            if (!owner.IsOwnedByServer)
            {
                return;
            }

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
                //在所有客户端播放倒计时
                owner.OnLeftTimeChangeClientRpc(_currentTime);
            }

            owner.ChangeStateClientRpc(GameStateEnum.GameOver);
        }

        public override void Exit()
        {
            _playingTokenSource?.Cancel();
        }
    }
}