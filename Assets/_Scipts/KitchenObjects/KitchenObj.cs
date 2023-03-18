using Kitchen.Interface;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObj : MonoBehaviour
    {
        public KitchenObjEnum objEnum;

        private ICanHoldKitchenObj _holder;

        public void SetHolder(ICanHoldKitchenObj canHoldKitchenObj)
        {
            if (canHoldKitchenObj.HasKitchenObj())
            {
                Debug.LogError("kitchenObjParent already has a KitchenObj");
            }
            _holder = canHoldKitchenObj;
            transform.SetParent(canHoldKitchenObj.GetTopSpawnPoint());
            transform.localPosition = Vector3.zero;
        }


        public ICanHoldKitchenObj GetHolder()
        {
            return _holder;
        }

        public void DestroySelf()
        {
            //ToDo 抽象成接口
            _holder.ClearKitchenObj();
            Destroy(gameObject);
        }
    }
}