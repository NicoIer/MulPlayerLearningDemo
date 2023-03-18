using UnityEngine;

namespace Kitchen.Interface
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