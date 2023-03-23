using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kitchen
{
    public class ReadyToStartState : GameState
    {
        private int defaultReadyCountDown => owner.readyCountDown;
        private int _readyCountDown;
        public Action<int> onCountDownChange;
        private bool _firstEnter = true;

        public override void Enter()
        {
            //开启计时器 readyCountDown秒后进入PlayingState
            //并且每过一秒减少一次readyCountDown 并且触发对应时间
            if (_firstEnter)
            {
                _readyCountDown = defaultReadyCountDown;
                _firstEnter = false;
            }

            _CountDown().Forget();
        }

        private CancellationTokenSource _countDownCts;

        public override void Exit()
        {
            _countDownCts?.Cancel();
        }

        private async UniTask _CountDown()
        {
            _countDownCts = new CancellationTokenSource();

            while (_readyCountDown > 0 && !_countDownCts.IsCancellationRequested)
            {
                onCountDownChange?.Invoke(_readyCountDown);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                _readyCountDown--;
            }

            stateMachine.Change<PlayingState>();
            return;
        }
    }
}