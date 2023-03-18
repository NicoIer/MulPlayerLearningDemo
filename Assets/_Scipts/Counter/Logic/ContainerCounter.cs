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
            _kitchenObjSo = DataTableManager.Sigleton.GetKitchenObjSo(objEnum);
            re.sprite = _kitchenObjSo.sprite;
        }

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObj())
            {
                return;
            }
            KitchenObjSpawner.SpawnKitchenObj(_kitchenObjSo,player);
            OnInteractEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}