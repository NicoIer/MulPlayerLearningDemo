using System;
using Kitchen.Interface;
using Nico;
using UnityEngine;

namespace Kitchen
{
    public class ClearCounter : MonoBehaviour, IInteract
    {
        [field: SerializeField] public Transform topSpawnPoint { get; private set; }
        [field: SerializeField] public KitchenObjEnum objEnum { get; private set; }
        private KitchenObj _kitchenObj;
        public ClearCounter nextClearCounter;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_kitchenObj != null && Player.Player.Singleton.selectCounterController.SelectedCounter == this)
                {
                    _kitchenObj.SetClearCounter(nextClearCounter); //设置物体的柜子
                    nextClearCounter.SetKitchenObj(_kitchenObj); //设置柜子的物体
                    _kitchenObj = null;
                }
            }
        }

        public void Interact()
        {
            if (_kitchenObj == null)
            {
                var kitchenObj = ObjectPoolManager.Singleton.GetObject(objEnum.ToString()).GetComponent<KitchenObj>();
                kitchenObj.SetClearCounter(this); //设置物体的柜子

                _kitchenObj = kitchenObj; //把物体设置到当前的柜子中
            }
            else
            {
                Debug.Log(_kitchenObj.GetClearCounter().name);
            }
        }

        public void SetKitchenObj(KitchenObj kitchenObj)
        {
            Debug.Log($"counter:{name} set kitchenObj to {kitchenObj}");
            _kitchenObj = kitchenObj;
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
}