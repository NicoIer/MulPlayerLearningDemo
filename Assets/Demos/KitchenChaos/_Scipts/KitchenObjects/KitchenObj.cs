using System;
using Kitchen;
using Nico.Components;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen
{
    public class KitchenObj : NetworkBehaviour
    {
        public KitchenObjEnum objEnum;

        protected ICanHoldKitchenObj holder;

        [field: SerializeField] public TransformFollower follower { get; private set; }

        private void Awake()
        {
            follower = GetComponent<TransformFollower>();
        }

        public void SetHolder(ICanHoldKitchenObj iholder)
        {
            follower.SetFollowTarget(iholder.GetHoldTransform());
            holder = iholder;
        }


        public ICanHoldKitchenObj GetHolder()
        {
            return holder;
        }

        public void ClearHolder()
        {
            holder.ClearKitchenObj();
            holder = null;
        }
        
    }
}