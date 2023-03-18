using System;
using Kitchen.Interface;
using Nico;
using UnityEngine;

namespace Kitchen
{
    public class ContainerCounter : BaseCounter
    {
        private KitchenObjSo _kitchenObjSo;
        public KitchenObjEnum objEnum;
        public event EventHandler OnInteractEvent;
        public SpriteRenderer re;


        private void Start()
        {
            //ToDO 这里的方式不是很好
            _kitchenObjSo = DataTableManager.Sigleton.Get(objEnum);
            re.sprite = _kitchenObjSo.sprite;
        }

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObj())
            {
                return;
            }

            var newKitchenObj = Instantiate(_kitchenObjSo.prefab)
                .GetComponent<KitchenObj>(); //从对象池中获取物体(从柜子中拿出物体)
            newKitchenObj.SetHolder(player); //设置物体的持有者
            player.SetKitchenObj(newKitchenObj); //设置持有者的物体
            OnInteractEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}