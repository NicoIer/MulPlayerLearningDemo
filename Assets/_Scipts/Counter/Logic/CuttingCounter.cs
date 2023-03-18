using System;
using System.Collections.Generic;
using Nico;
using UnityEngine;

namespace Nico
{
    public class CuttingCounter : BaseCounter, IInteractAlternate
    {
        public int cuttingCount = 0;
        private ProgressBarUI _progressBarUI;
        public event Action OnCuttingEvent;
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
                cuttingCount = 0;
                _progressBarUI.SetProgress(0);
                
                KitchenObjOperator.PutKitchenObj(player, this);
                return;
            }

            //玩家没有持有物体，当前柜子有物体 -> 拿起物体 TODO 完成切的逻辑
            if (!player.HasKitchenObj() && HasKitchenObj())
            {
                KitchenObjOperator.PutKitchenObj(this, player);
                return;
            }
        }


        //交互逻辑 这里是切菜的逻辑
        public void InteractAlternate(Player.Player player)
        {
            if (!HasKitchenObj()) return;

            var currentKitchenObj = GetKitchenObj();
            var nextKitchenObjSo = DataTableManager.Sigleton.GetCutKitchenObjSo(currentKitchenObj.objEnum);

            if (nextKitchenObjSo == null) return;
            //获取最大切菜次数
            var maxCuttingCount = DataTableManager.Sigleton.GetCuttingCount(currentKitchenObj.objEnum);
            //切菜
            ++cuttingCount;
            //触发切菜事件
            OnCuttingEvent?.Invoke();
            //设置进度条
            _progressBarUI.SetProgress((float) cuttingCount / maxCuttingCount);
            //如果切完了
            if (cuttingCount >= maxCuttingCount)
            {
                currentKitchenObj.DestroySelf();
                var obj = KitchenObjOperator.SpawnKitchenObj(nextKitchenObjSo, this);
                cuttingCount = 0;
            }
        }
    }
}