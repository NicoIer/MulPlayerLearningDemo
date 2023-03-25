using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nico.Components;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen
{
    public class StoveCounter : BaseCounter, IInteractAlternate
    {
        private CancellationTokenSource _cookingCts;
        public event Action OnStartCooking;
        public event Action OnStopCooking;
        public event Action<KitchenObjEnum?> OnCookingStageChange;
        private ProgressBar _progressBarUI;

        protected override void Awake()
        {
            base.Awake();
            _progressBarUI = transform.Find("ProgressBarUI").GetComponent<ProgressBar>();
        }

        public override void Interact(Player.Player player)
        {
            //玩家持有物体，当前柜子没有物体 -> 放置物体
            if (player.HasKitchenObj() && !HasKitchenObj())
            {
                KitchenObjOperator.PutKitchenObj(player, this);
                return;
            }

            //玩家没有持有物体，当前柜子有物体 -> 拿起物体
            if (!player.HasKitchenObj() && HasKitchenObj())
            {
                if (isCooking)
                {
                    _StopCookingServerRpc();
                }

                KitchenObjOperator.PutKitchenObj(this, player);
                return;
            }


            if (!player.HasKitchenObj() || !HasKitchenObj()) return;
            //都有物体
            //尝试进行盘子放置的操作
            //失败则直接返回
            if (!CounterOperator.TryPlateOperator(player, this)) return;

            //操作成功则停止烹饪
            if (isCooking)
            {
                _StopCookingServerRpc();
            }
        }

        public bool isCooking;

        public void InteractAlternate(Player.Player player)
        {
            //如果当前柜子有物体，且当前不在烹饪 且 物体可以被烹饪 则开启烹饪任务
            if (HasKitchenObj() && !isCooking && KitchenObjOperator.CanCook(kitchenObj))
            {
                StartCookingServerRpc(); //通知服务器开启烹饪
                return;
            }

            //如果当前柜子有物体 且在烹饪 则停止烹饪
            if (HasKitchenObj() && isCooking)
            {
                _StopCookingServerRpc(); //通知服务器停止烹饪
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void _StopCookingServerRpc()
        {
            _cookingCts?.Cancel();
            _StopCookingClientRpc();
        }

        [ClientRpc]
        private void _StopCookingClientRpc()
        {
            isCooking = false;
            OnStopCooking?.Invoke();
            OnCookingStageChange?.Invoke(null);
            _progressBarUI.Hide();
        }

        [ServerRpc(RequireOwnership = false)]
        private void StartCookingServerRpc()
        {
            _Cooking().Forget();
        }


        private async UniTask _Cooking()
        {
            if (!IsServer)
                throw new Exception("只能在服务端执行 Cooking任务");

            _cookingCts = new CancellationTokenSource();
            _OnStartCookingClientRpc();
            _CookingStageChangeClientRpc(kitchenObj.objEnum);
            while (KitchenObjOperator.CanCook(kitchenObj) && !_cookingCts.IsCancellationRequested)
            {
                //获取此次烹饪需要的时间
                float cookTime = DataTableManager.Sigleton.GetCookingTime(kitchenObj.objEnum);
                //开始烹饪 同时 更新进度条
                var startTime = Time.time;
                while (Time.time - startTime < cookTime && !_cookingCts.IsCancellationRequested)
                {
                    await UniTask.WaitForFixedUpdate(cancellationToken: _cookingCts.Token);
                    _SetProgressClientRpc((Time.time - startTime) / cookTime);
                }


                KitchenObjOperator.Cook(kitchenObj, this); //这个操作只能在服务端执行 否则会烹饪两次


                //烹饪阶段改变 
                Debug.Log(kitchenObj.objEnum);
                _CookingStageChangeClientRpc(kitchenObj.objEnum);
            }

            _CookingStageChangeClientRpc(kitchenObj.objEnum);
            _OnStopCookingClientRpc();
        }

        [ClientRpc]
        private void _SetProgressClientRpc(float progress)
        {
            _progressBarUI.SetProgress(progress);
        }
        [ClientRpc]
        private void _OnStartCookingClientRpc()
        {
            isCooking = true;
            OnStartCooking?.Invoke();
        }

        [ClientRpc]
        private void _OnStopCookingClientRpc()
        {
            isCooking = false;
            OnStopCooking?.Invoke();
        }

        [ClientRpc]
        private void _CookingStageChangeClientRpc(KitchenObjEnum kitchenObjEnum)
        {
            OnCookingStageChange?.Invoke(kitchenObjEnum);
        }
    }
}