using System;
using Kitchen.Interface;
using Nico;
using UnityEngine;

namespace Kitchen
{
    public class ContainerCounter : BaseCounter
    {
        [field: SerializeField] public KitchenObjEnum objEnum { get; private set; }
        public event EventHandler OnInteractEvent;

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObj())
            {
                return;
            }
            
            var newKitchenObj = ObjectPoolManager.Singleton.GetObject(objEnum.ToString())
                .GetComponent<KitchenObj>(); //从对象池中获取物体(从柜子中拿出物体)
            newKitchenObj.SetHolder(player); //设置物体的持有者
            player.SetKitchenObj(newKitchenObj); //设置持有者的物体
            OnInteractEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}