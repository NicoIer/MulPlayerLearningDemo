using System;
using Kitchen;
using UnityEngine;

namespace Kitchen
{
    public abstract class BaseCounter : MonoBehaviour, IInteract, ICanHoldKitchenObj
    {
        protected Transform topSpawnPoint { get; set; }
        protected KitchenObj kitchenObj;

        protected virtual void Awake()
        {
            topSpawnPoint = transform.Find("TopPoint");
        }

        public abstract void Interact(Player.Player player);

        public virtual Transform GetTopSpawnPoint()
        {
            return topSpawnPoint;
        }

        public virtual KitchenObj GetKitchenObj()
        {
            return kitchenObj;
        }

        public virtual void SetKitchenObj(KitchenObj newKitchenObj)
        {
            kitchenObj = newKitchenObj;
        }

        public virtual bool HasKitchenObj()
        {
            return kitchenObj != null;
        }

        public virtual void ClearKitchenObj()
        {
            kitchenObj = null;
        }
    }
}