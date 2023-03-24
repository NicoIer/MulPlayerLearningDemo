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
            _SpawnKitObjServerRpc(kitchenObjEnum, holder.GetNetworkObject());
        }

        [ServerRpc(RequireOwnership = false)]
        private void _SpawnKitObjServerRpc(KitchenObjEnum kitchenObjEnum, NetworkObjectReference holderRef)
        {
            var so = DataTableManager.Sigleton.GetKitchenObjSo(kitchenObjEnum);
            
            
            var obj = Instantiate(so.prefab).GetComponent<KitchenObj>();//生成 KitObj 并且获取对应脚本
            var netObj = obj.GetComponent<NetworkObject>();//获取物体网络组件
            netObj.Spawn(true);//在网络上生成这个物体 生成的物体会在所有客户端生成
            
            //
            _SetHolderClientRpc(holderRef,netObj);
        }
        
        [ClientRpc]
        private void _SetHolderClientRpc(NetworkObjectReference holderRef,NetworkObjectReference objReference)
        {
            holderRef.TryGet(out NetworkObject holderObj);
            var holder = holderObj.GetComponent<ICanHoldKitchenObj>();
            if (holder.HasKitchenObj())
            {
                Debug.LogWarning(
                    $"{holder}, type[{holder.GetType()}] already has:{this} type[{GetType()}]" +
                    " it will be replaced by this"
                );
            }
            
            objReference.TryGet(out NetworkObject obj);
            var kitchenObj = obj.GetComponent<KitchenObj>();

            kitchenObj.SetHolder(holder);
            holder.SetKitchenObj(kitchenObj);
        }


        [ServerRpc(RequireOwnership = false)]
        public void PutKitObjServerRpc(NetworkObjectReference putterRef, NetworkObjectReference recieverRef)
        {
            _PutKitObjClientRpc(putterRef, recieverRef);
        }
        
        [ClientRpc]
        
        private void _PutKitObjClientRpc(NetworkObjectReference putterRef, NetworkObjectReference recieverRef)
        {
            putterRef.TryGet(out NetworkObject putterObj);
            recieverRef.TryGet(out NetworkObject recieverObj);
            var putter = putterObj.GetComponent<ICanHoldKitchenObj>();
            var reciever = recieverObj.GetComponent<ICanHoldKitchenObj>();
            
            var obj = putter.GetKitchenObj();
            obj.SetHolder(reciever);
            reciever.SetKitchenObj(obj);
            putter.ClearKitchenObj();
        }
        

    }
}