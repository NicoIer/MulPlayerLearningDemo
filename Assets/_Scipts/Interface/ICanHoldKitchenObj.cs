using UnityEngine;

namespace Kitchen
{
    public interface ICanHoldKitchenObj
    {
        Transform GetTopSpawnPoint();
        KitchenObj GetKitchenObj();
        void SetKitchenObj(KitchenObj newKitchenObj);
        bool HasKitchenObj();
        void ClearKitchenObj();
    }
}