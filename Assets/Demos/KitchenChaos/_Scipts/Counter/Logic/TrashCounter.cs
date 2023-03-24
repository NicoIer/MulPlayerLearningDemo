using System;
using Unity.Netcode;
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
                KitchenObjOperator.DestroyKitchenObj(player.GetKitchenObj());
                TrashObjServerRpc(transform.position);
            }
        }
        [ServerRpc(RequireOwnership = false)]
        private void TrashObjServerRpc(Vector3 position)
        {
            TranshObjClientRpc(position);
        }
        [ClientRpc]
        private void TranshObjClientRpc(Vector3 position)
        {
            OnAnyObjTrashed?.Invoke(position);
        }

    }
}