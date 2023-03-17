using System;
using Kitchen.Interface;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : ICabHoldKitchenObj
    {
        public Transform GetTopSpawnPoint()
        {
            return topSpawnPoint;
        }

        public KitchenObj GetKitchenObj()
        {
            return _kitchenObj;
        }

        public void SetKitchenObj(KitchenObj kitchenObj)
        {
            this._kitchenObj = kitchenObj;
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