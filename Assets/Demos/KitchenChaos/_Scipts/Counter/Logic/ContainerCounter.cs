using System;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen
{
    public class ContainerCounter : BaseCounter
    {
        private KitchenObjSo _kitchenObjSo;
        public KitchenObjEnum objEnum;
        public event Action OnInteractEvent;
        public SpriteRenderer re;


        private void Start()
        {
            //ToDO 这里的方式不是很好
            _kitchenObjSo = DataTableManager.Sigleton.GetKitchenObjSo(objEnum);
            re.sprite = _kitchenObjSo.sprite;
        }

        public override void Interact(Player.Player player)
        {
            Debug.Log(player + "  " + player.NetworkObjectId + "尝试获取道具" + $"它是否有道具:{player.HasKitchenObj()}");
            if (player.HasKitchenObj())
            {
                Debug.Log("获取失败");
                return;
            }

            Debug.Log("请求生成道具");
            KitchenObjOperator.SpawnKitchenObjRpc(_kitchenObjSo.kitchenObjEnum, player);
            _OnInteractServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void _OnInteractServerRpc()
        {
            _OnInteractClientRpc();
        }

        [ClientRpc]
        private void _OnInteractClientRpc()
        {
            Debug.Log("交互事件触发");
            OnInteractEvent?.Invoke();
        }
    }
}