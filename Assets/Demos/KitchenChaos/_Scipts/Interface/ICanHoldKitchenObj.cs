using Unity.Netcode;
using UnityEngine;

namespace Kitchen
{
    public interface ICanHoldKitchenObj
    {
        Transform GetHoldTransform();
        KitchenObj GetKitchenObj();
        void SetKitchenObj(KitchenObj newKitchenObj);
        bool HasKitchenObj();
        void ClearKitchenObj();
        
        NetworkObject GetNetworkObject();
    }
}