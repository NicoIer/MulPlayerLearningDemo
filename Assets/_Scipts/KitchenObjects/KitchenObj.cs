using Kitchen.Interface;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObj : MonoBehaviour
    {
        [SerializeField] private KitchenObjSo kitchenObjSo;
        private ICanHoldKitchenObj _canHoldKitchenObj;

        public void SetHolder(ICanHoldKitchenObj canHoldKitchenObj)
        {
            if (canHoldKitchenObj.HasKitchenObj())
            {
                Debug.LogError("kitchenObjParent already has a KitchenObj");
            }

            _canHoldKitchenObj = canHoldKitchenObj;
            transform.SetParent(canHoldKitchenObj.GetTopSpawnPoint());
            transform.localPosition = Vector3.zero;
        }
        

        public ICanHoldKitchenObj GetClearCounter()
        {
            return _canHoldKitchenObj;
        }
    }
}