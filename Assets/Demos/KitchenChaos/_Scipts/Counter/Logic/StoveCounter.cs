using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nico.DesignPattern.Singleton.Network;
using UnityEngine;

namespace Kitchen
{
    public class StoveCounter : BaseCounter, IInteractAlternate
    {
        private CancellationTokenSource _cookingCts;
        public event EventHandler OnStartCooking;
        public event EventHandler OnStopCooking;
        public event Action<KitchenObjEnum?> OnCookingStageChange;
        private ProgressBarUI _progressBarUI;

        protected override void Awake()
        {
            base.Awake();
            _progressBarUI = transform.Find("ProgressBarUI").GetComponent<ProgressBarUI>();
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
                    _StopCooking();
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
                _StopCooking();
            }
        }

        public bool isCooking = false;

        public void InteractAlternate(Player.Player player)
        {
            //如果当前柜子有物体，且当前不在烹饪 且 物体可以被烹饪 则开启烹饪任务
            if (HasKitchenObj() && !isCooking && KitchenObjOperator.CanCook(kitchenObj))
            {
                _Cooking().Forget();
                return;
            }

            //如果当前柜子有物体 且在烹饪 则停止烹饪
            if (HasKitchenObj() && isCooking)
            {
                _StopCooking();
            }
        }


        private void _StopCooking()
        {
            _cookingCts?.Cancel();
            isCooking = false;
            OnStopCooking?.Invoke(this, EventArgs.Empty);
            OnCookingStageChange?.Invoke(null);
            _progressBarUI.Hide();
        }

        private async UniTask _Cooking()
        {
            _cookingCts = new CancellationTokenSource();
            isCooking = true;
            OnStartCooking?.Invoke(this, EventArgs.Empty);
            OnCookingStageChange?.Invoke(kitchenObj.objEnum);
            while (KitchenObjOperator.CanCook(kitchenObj) && !_cookingCts.IsCancellationRequested)
            {
                
                //获取此次烹饪需要的时间
                float cookTime = DataTableManager.Sigleton.GetCookingTime(kitchenObj.objEnum);
                //开始烹饪 同时 更新进度条
                var startTime = Time.time;
                while (Time.time - startTime < cookTime && !_cookingCts.IsCancellationRequested)
                {
                    await UniTask.WaitForFixedUpdate(cancellationToken: _cookingCts.Token);
                    _progressBarUI.SetProgress((Time.time - startTime) / cookTime);
                }

                KitchenObjOperator.Cook(kitchenObj, this);
                //烹饪阶段改变 
                OnCookingStageChange?.Invoke(kitchenObj.objEnum);

            }

            OnCookingStageChange?.Invoke(kitchenObj.objEnum);
            OnStopCooking?.Invoke(this, EventArgs.Empty);

            isCooking = false;
        }
    }
}