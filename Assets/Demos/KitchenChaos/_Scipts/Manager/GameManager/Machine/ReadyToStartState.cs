using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kitchen
{
    public class ReadyToStartState : GameState
    {
        public static readonly GameStateEnum stateEnum = GameStateEnum.ReadyToStart;
        private int defaultReadyCountDown => owner.setting.readyCountDown;//默认准备时间
        private int _leftCountDown;//
        private bool _firstEnter = true;

        public override void Enter()
        {
            if (!owner.IsOwnedByServer) //只有Server才能开启定时器 任务
            {
                return;
            }

            //开启计时器 readyCountDown秒后进入PlayingState
            //并且每过一秒减少一次readyCountDown 并且触发对应时间
            if (_firstEnter)
            {
                _leftCountDown = defaultReadyCountDown;
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

            while (_leftCountDown > 0 && !_countDownCts.IsCancellationRequested)
            {
                //通知所有客户端进行倒计时播放
                owner.OnCountDownChangeClientRpc(_leftCountDown);
                //服务器倒计时--
                _leftCountDown--;
                
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            owner.ChangeStateClientRpc(GameStateEnum.Playing);
            stateMachine.Change<PlayingState>();
            return;
        }
    }
}