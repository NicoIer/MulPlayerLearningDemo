using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
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
        public event Action<int> OnPlateCountChanged;

        private void Start()
        {
            GameManager.Instance.stateMachine.onStateChange += _OnGameStateChange;
        }

        private void _OnGameStateChange(GameState arg1, GameState arg2)
        {
            if (!IsServer)
                return;
            if (arg2 is PlayingState)
            {
                _StartGeneratePlate();
            }
            else
            {
                _TryStopGeneratePlate();
            }
        }


        private void OnDisable()
        {
            _TryStopGeneratePlate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _TryStopGeneratePlate();
        }

        private void _StartGeneratePlate()
        {
            if (!IsServer) //只有服务器端才会生成盘
            {
                return;
            }

            if (!_isGenerating)
            {
                _GeneratePlate().Forget();
            }
        }

        private void _TryStopGeneratePlate()
        {
            _plateGenerateCts?.Cancel();
            _isGenerating = false;
        }

        private async UniTask _GeneratePlate()
        {
            if (!IsServer) return;

            _isGenerating = true;
            while (true)
            {
                _plateGenerateCts = new CancellationTokenSource();
                await UniTask.Delay((int)(spawnInterval * 1000), cancellationToken: _plateGenerateCts.Token);
                _SpawnPlateServerRpc(); // 生成盘子ServerRpc
                if (plateCount >= plateMaxCount)
                    break;
                if (_plateGenerateCts.IsCancellationRequested) break;
            }

            _isGenerating = false;
        }

        [ServerRpc]
        private void _SpawnPlateServerRpc()
        {
            //如果盘子数量小于最大值，则生成一个盘子
            if (plateCount < plateMaxCount)
            {
                _SpawnPlateClientRpc();
            }
        }

        [ClientRpc]
        private void _SpawnPlateClientRpc()
        {
            ++plateCount;
            OnPlateCountChanged?.Invoke(plateCount);
        }

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObj()) return;
            if (plateCount <= 0) return;

            //当拿走盘子后 重新开启生成盘子的任务
            KitchenObjOperator.SpawnKitchenObjRpc(_kitchenObjEnum, player);
            RemovePlateServerRpc();
            OnPlateCountChanged?.Invoke(plateCount);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RemovePlateServerRpc()
        {
            RemovePlateClientRpc();
            if (!_isGenerating && IsServer)
                _GeneratePlate().Forget();
        }

        [ClientRpc]
        public void RemovePlateClientRpc()
        {
            --plateCount;
            OnPlateCountChanged?.Invoke(plateCount);
        }
    }
}