using System;
using Kitchen.Interface;
using Nico;
using UnityEngine;

namespace Kitchen
{
    public partial class ClearCounter : ICabHoldKitchenObj
    {
        public void SetKitchenObj(KitchenObj kitchenObj)
        {
            _kitchenObj = kitchenObj;
        }

        public Transform GetTopSpawnPoint()
        {
            return topSpawnPoint;
        }

        public KitchenObj GetKitchenObj()
        {
            return _kitchenObj;
        }

        public bool HasKitchenObj()
        {
            return _kitchenObj != null;
        }

        public void ClearKitchenObj()
        {
            _kitchenObj = null;
        }
    }

    public partial class ClearCounter : MonoBehaviour, IInteract
    {
        [field: SerializeField] public Transform topSpawnPoint { get; private set; }
        [field: SerializeField] public KitchenObjEnum objEnum { get; private set; }
        private KitchenObj _kitchenObj;

        private void Awake()
        {
            topSpawnPoint = transform.Find("KitchenObjHoldPoint");
        }

        public void Interact(Player.Player player)
        {
            if (_kitchenObj == null)
            {
                var kitchenObj = ObjectPoolManager.Singleton.GetObject(objEnum.ToString()).GetComponent<KitchenObj>();
                kitchenObj.SetKitchenParent(this); //设置物体的柜子

                _kitchenObj = kitchenObj; //把物体设置到当前的柜子中
            }
            else
            {
                _kitchenObj.SetKitchenParent(player); //设置物体的柜子
                player.SetKitchenObj(_kitchenObj); //设置柜子的物体
                _kitchenObj = null;
            }
        }
    }
}