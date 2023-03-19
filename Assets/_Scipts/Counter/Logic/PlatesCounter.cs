using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kitchen
{
    public class PlatesCounter : BaseCounter
    {
        private CancellationTokenSource _plateGenerateCts;
        private readonly KitchenObjEnum _kitchenObjEnum = KitchenObjEnum.Plate;
        public float spawnInterval = 4f;
        public int plateCount = 0;
        public int plateMaxCount = 5;
        private bool _isGenerating = false;
        public event EventHandler<int> OnPlateCountChanged;


        private void OnEnable()
        {
            //开启定时器 每到时间后自动生成一个盘子
            _GeneratePlate().Forget();
        }

        private void OnDisable()
        {
            _StopGeneratePlate();
        }

        private void _StopGeneratePlate()
        {
            _plateGenerateCts?.Cancel();
            _isGenerating = false;
        }
        private async UniTask _GeneratePlate()
        {
            _isGenerating = true;
            while (true)
            {
                _plateGenerateCts = new CancellationTokenSource();
                await UniTask.Delay((int)(spawnInterval * 1000), cancellationToken: _plateGenerateCts.Token);
                //尝试生成一个盘子
                ++plateCount;
                if (plateCount < plateMaxCount)
                {
                    OnPlateCountChanged?.Invoke(this, plateCount);
                    // KitchenObjOperator.SpawnKitchenObj(_kitchenObjEnum, this);
                }
                else
                {
                    //当满了之后 就结束任务
                    plateCount = plateMaxCount;
                    break;
                }

                if (_plateGenerateCts.IsCancellationRequested) break;
            }

            _isGenerating = false;
        }

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObj()) return;
            if (plateCount <= 0) return;

            //当拿走盘子后 重新开启生成盘子的任务
            --plateCount;
            if (!_isGenerating)
                _GeneratePlate().Forget();

            OnPlateCountChanged?.Invoke(this, plateCount);
            KitchenObjOperator.SpawnKitchenObj(_kitchenObjEnum, player);
        }
    }
}