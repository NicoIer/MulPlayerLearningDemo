using Kitchen.Interface;
using Nico;
using UnityEngine;

namespace Kitchen
{
    public  static class KitchenObjSpawner
    {
        public static KitchenObj SpawnKitchenObj(KitchenObjSo kitchenObjSo,ICanHoldKitchenObj holder)
        {
            var obj = Object.Instantiate(kitchenObjSo.prefab).GetComponent<KitchenObj>();
            obj.SetHolder(holder);
            holder.SetKitchenObj(obj);
            //ToDo 下面的操作在SetHolder已经执行过了
            // obj.transform.SetParent(holder.GetTopSpawnPoint());
            // obj.transform.localPosition = Vector3.zero;
            return obj;
        }
    }
}