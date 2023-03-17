using Kitchen.Interface;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObj : MonoBehaviour
    {
        [SerializeField] private KitchenObjSo kitchenObjSo;
        private ICabHoldKitchenObj _cabHoldKitchenObj;

        public void SetKitchenParent(ICabHoldKitchenObj cabHoldKitchenObj)
        {
            if (cabHoldKitchenObj.HasKitchenObj())
            {
                Debug.LogError("kitchenObjParent already has a KitchenObj");
            }

            _cabHoldKitchenObj = cabHoldKitchenObj;
            transform.SetParent(cabHoldKitchenObj.GetTopSpawnPoint());
            transform.localPosition = Vector3.zero;
        }
        

        public ICabHoldKitchenObj GetClearCounter()
        {
            return _cabHoldKitchenObj;
        }
    }
}