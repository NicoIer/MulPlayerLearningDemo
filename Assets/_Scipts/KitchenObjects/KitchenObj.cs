using Kitchen;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObj : MonoBehaviour
    {
        public KitchenObjEnum objEnum;

        protected ICanHoldKitchenObj holder;

        public void SetHolder(ICanHoldKitchenObj canHoldKitchenObj)
        {
            if (canHoldKitchenObj.HasKitchenObj())
            {
                Debug.LogError("kitchenObjParent already has a KitchenObj");
            }
            holder = canHoldKitchenObj;
            transform.SetParent(canHoldKitchenObj.GetTopSpawnPoint());
            transform.localPosition = Vector3.zero;
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