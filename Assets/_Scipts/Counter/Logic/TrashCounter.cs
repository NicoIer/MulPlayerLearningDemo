using System;
using UnityEngine;

namespace Kitchen
{
    public class TrashCounter : BaseCounter
    { 
        public static event Action<Vector3> OnAnyObjTrashed;
        public override void Interact(Player.Player player)
        {
            //将玩家手上的东西销毁掉
            if (player.HasKitchenObj())
            {
                player.GetKitchenObj().DestroySelf();
                OnAnyObjTrashed?.Invoke(transform.position);
            }
        }

       
    }
}