using System;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : ICanHoldKitchenObj
    {
        
        public Transform GetHoldTransform()
        {
            return topSpawnPoint;
        }

        public KitchenObj GetKitchenObj()
        {
            return _kitchenObj;
        }

        public void SetKitchenObj(KitchenObj newKitchenObj)
        {
            if (newKitchenObj != null)
            {
                OnAnyPickUpSomeThing?.Invoke(transform.position);
            }
            _kitchenObj = newKitchenObj;
        }

        public bool HasKitchenObj()
        {
            return this._kitchenObj != null;
        }

        public void ClearKitchenObj()
        {
            this._kitchenObj = null;
        }
    }
}