using UnityEngine;

namespace Kitchen
{
    public class KitchenObj : MonoBehaviour
    {
        [SerializeField] private KitchenObjSo kitchenObjSo;
        private ClearCounter _clearCounter;

        public void SetClearCounter(ClearCounter newClearCounter)
        {
            if (_clearCounter != null)
            {
                _clearCounter = null;
            }
            //如果新的柜子已经有物体了，就报错
            if (newClearCounter.HasKitchenObj())
            {
                Debug.LogError($"ClearCounter:{newClearCounter.name} already has a KitchenObj");
            }
            
            
            //设置当前物品的柜子
            _clearCounter = newClearCounter;
            
            transform.SetParent(newClearCounter.topSpawnPoint);
            transform.localPosition = Vector3.zero;
        }

        public ClearCounter GetClearCounter()
        {
            return _clearCounter;
        }
    }
}