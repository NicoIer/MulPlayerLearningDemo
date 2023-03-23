using System;
using Kitchen;
using Nico.Components;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObj : NetworkBehaviour
    {
        public KitchenObjEnum objEnum;

        protected ICanHoldKitchenObj holder;

        private TransformFollower follower;

        private void Awake()
        {
            follower = GetComponent<TransformFollower>();
        }

        public void SetHolder(ICanHoldKitchenObj canHoldKitchenObj)
        {
            _SetHolderServerRpc(canHoldKitchenObj.GetNetworkObject());
        }

        [ServerRpc(RequireOwnership = false)]
        private void _SetHolderServerRpc(NetworkObjectReference holderRef)
        {
            _SetHolderClientRpc(holderRef);
        }

        [ClientRpc]
        private void _SetHolderClientRpc(NetworkObjectReference holderRef)
        {
            holderRef.TryGet(out NetworkObject holderObj);
            var canHoldKitchenObj = holderObj.GetComponent<ICanHoldKitchenObj>();
            if (canHoldKitchenObj.HasKitchenObj())
            {
                Debug.Log("holder: " + canHoldKitchenObj + " already has:" + canHoldKitchenObj.GetKitchenObj());
                Debug.LogError("kitchenObjParent already has a KitchenObj");
            }

            holder = canHoldKitchenObj;
            holder.SetKitchenObj(this);//这一步很重要 因为 holder是否持有物体 也是需要网络同步的的!!!
            //TODO DEBUG
            follower.SetFollowTarget(canHoldKitchenObj.GetTopSpawnPoint());
        }

        public ICanHoldKitchenObj GetHolder()
        {
            return holder;
        }

        public void ClearHolder()
        {
            holder.ClearKitchenObj();
            holder = null;
        }

        public void DestroySelf()
        {
            //ToDo 抽象成接口
            ClearHolder();
            Destroy(gameObject);
        }
    }
}