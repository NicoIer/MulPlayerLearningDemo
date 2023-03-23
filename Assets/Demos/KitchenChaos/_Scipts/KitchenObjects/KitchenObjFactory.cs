using System;
using System.Collections.Generic;
using Nico.Network;
using Nico.Network.Singleton;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObjFactory : NetSingleton<KitchenObjFactory>
    {
        public void CreateKitchenObj(KitchenObjEnum kitchenObjEnum, ICanHoldKitchenObj holder)
        {
            SpawnKitObjServerRpc(kitchenObjEnum, holder.GetNetworkObject());
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnKitObjServerRpc(KitchenObjEnum kitchenObjEnum, NetworkObjectReference holderRef)
        {
            var so = DataTableManager.Sigleton.GetKitchenObjSo(kitchenObjEnum);

            Transform objTransform = Instantiate(so.prefab).transform;//生成物体
            
            var obj = objTransform.GetComponent<KitchenObj>();//获取物体脚本
            
            //TAG Spawn 时 所有客户端都会生成这个物体
            var netObj = obj.GetComponent<NetworkObject>();//获取物体网络组件
            netObj.Spawn(true);
            holderRef.TryGet(out NetworkObject holderObj);
            var holder = holderObj.GetComponent<ICanHoldKitchenObj>();
            //设置Parent的这一步 需要在所有客户端调用 所以需要RPC
            obj.SetHolder(holder);
            holder.SetKitchenObj(obj);
        }

        [ClientRpc]
        public void SetParentClientRpc()
        {
            
        }
    }
}