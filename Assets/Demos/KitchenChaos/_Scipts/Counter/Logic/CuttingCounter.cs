using System;
using Nico.Components;
using Unity.Netcode;
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
                Debug.Log("玩家持有物体，当前案板没有物体 -> 放置物体");
                cuttingCount = 0;
                _progressBar.SetProgress(0);
                KitchenObjOperator.PutKitchenObj(player, this);
                return;
            }

            //玩家没有持有物体，当前柜子有物体 -> 拿起物体
            if (!player.HasKitchenObj() && HasKitchenObj())
            {
                Debug.Log("玩家没有持有物体，当前案板有物体 -> 拿起物体");
                cuttingCount = 0;
                _progressBar.SetProgress(0);
                KitchenObjOperator.PutKitchenObj(this, player);
                return;
            }

            if (CounterOperator.TryPlateOperator(player, this)) return;
        }


        //交互逻辑 这里是切菜的逻辑
        public void InteractAlternate(Player.Player player)
        {
            if (!HasKitchenObj()) return;
            
            var nextObj = DataTableManager.Sigleton.GetCutKitObj(kitchenObj.objEnum);
            if (nextObj == null) return;
            CuttingServerRpc(transform.position);
        }

        [ServerRpc(RequireOwnership = false)]
        public void CuttingServerRpc(Vector3 position)
        {
            CuttingClientRpc(position);
        }

        [ClientRpc]
        public void CuttingClientRpc(Vector3 position)
        {
            
            //获取当前物体的最大切菜次数
            var maxCuttingCount = DataTableManager.Sigleton.GetCuttingCount(kitchenObj.objEnum);
            
            //触发切菜事件
            ++cuttingCount;
            OnCuttingEvent?.Invoke();
            OnAnyCut?.Invoke(this, position);
            
            
            _progressBar.SetProgress((float)cuttingCount / maxCuttingCount);
            
            
            if (cuttingCount >= maxCuttingCount)
            {
                var nextObj = DataTableManager.Sigleton.GetCutKitObj(kitchenObj.objEnum);//获取切完后的物体
                KitchenObjOperator.DestroyKitchenObj(kitchenObj);//销毁当前物体
                Debug.Log("切菜完成,原先物体已经被销毁,准备生成新物体");
                KitchenObjOperator.SpawnKitchenObjRpc(nextObj.kitchenObjEnum, this);//生成切完后的物体
                cuttingCount = 0;
            }
        }
    }
}