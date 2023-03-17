using UnityEngine;

namespace Kitchen.Interface
{
    public interface ICabHoldKitchenObj
    {
        Transform GetTopSpawnPoint();
        KitchenObj GetKitchenObj();
        void SetKitchenObj(KitchenObj kitchenObj);
        bool HasKitchenObj();
        void ClearKitchenObj();
    }
}