﻿using System;
using Nico.Components;
using UnityEngine;

namespace Kitchen
{
    public class CuttingCounter : BaseCounter, IInteractAlternate
    {
        public int cuttingCount = 0;
        private ProgressBar _progressBar;
        public event Action OnCuttingEvent;
        public static event EventHandler<Vector3> OnAnyCut; 

        protected override void Awake()
        {
            base.Awake();
            _progressBar = transform.Find("ProgressBarUI").GetComponent<ProgressBar>();
        }

        public override void Interact(Player.Player player)
        {
            //玩家持有物体，当前柜子没有物体 -> 放置物体
            if (player.HasKitchenObj() && !HasKitchenObj())
            {
                cuttingCount = 0;
                _progressBar.SetProgress(0);
                KitchenObjOperator.PutKitchenObj(player, this);
                return;
            }

            //玩家没有持有物体，当前柜子有物体 -> 拿起物体
            if (!player.HasKitchenObj() && HasKitchenObj())
            {
                cuttingCount = 0;
                _progressBar.SetProgress(0);
                KitchenObjOperator.PutKitchenObj(this, player);
                return;
            }
            
            if(CounterOperator.TryPlateOperator(player, this)) return;
        }


        //交互逻辑 这里是切菜的逻辑
        public void InteractAlternate(Player.Player player)
        {
            if (!HasKitchenObj()) return;

            var currentKitchenObj = GetKitchenObj();
            var cutKitchenObjSo = DataTableManager.Sigleton.GetCutKitchenObjSo(currentKitchenObj.objEnum);
            //ToDo 这里判断是否可以切菜
            if (cutKitchenObjSo == null) return;
            //获取最大切菜次数
            var maxCuttingCount = DataTableManager.Sigleton.GetCuttingCount(currentKitchenObj.objEnum);
            //切菜
            ++cuttingCount;
            //触发切菜事件
            OnCuttingEvent?.Invoke();
            OnAnyCut?.Invoke(this, transform.position);
            //设置进度条
            _progressBar.SetProgress((float)cuttingCount / maxCuttingCount);
            //如果切完了
            if (cuttingCount >= maxCuttingCount)
            {
                KitchenObjOperator.DestroyKitchenObj(currentKitchenObj);
                KitchenObjOperator.SpawnKitchenObjRpc(cutKitchenObjSo.kitchenObjEnum, this);
                cuttingCount = 0;
            }
        }
    }
}