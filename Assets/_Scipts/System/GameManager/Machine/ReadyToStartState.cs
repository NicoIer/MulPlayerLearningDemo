using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kitchen
{
    public class ReadyToStartState : GameState
    {
        private int defaultReadyCountDown => owner.readyCountDown;
        private int _readyCountDown;
        public Action<int> onCountDownChange;
        public Action OnGameReadyToStart;
        
        public override void Enter()
        {
            _readyCountDown = defaultReadyCountDown;
            //开启计时器 readyCountDown秒后进入PlayingState
            //并且每过一秒减少一次readyCountDown 并且触发对应时间
            OnGameReadyToStart?.Invoke();
            _CountDown().Forget();
        }

        private async UniTask _CountDown()
        {
            while (_readyCountDown > 0)
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