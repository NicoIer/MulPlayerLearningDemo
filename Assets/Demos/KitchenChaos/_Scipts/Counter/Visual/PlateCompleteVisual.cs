using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class PlateCompleteVisual : MonoBehaviour
    {
        [Serializable]
        private struct ObjEnum2Obj
        {
            public KitchenObjEnum objEnum;
            public GameObject obj;
        }

        //TODO 当前的视觉效果是有一个 包含了所有物体的盘子 通过激活其中的一些部分 来实现的 不是很优雅 
        private Plate _plate;
        [SerializeField] private List<ObjEnum2Obj> _objEnum2Objs = new List<ObjEnum2Obj>();

        private void Awake()
        {
            _plate = GetComponentInParent<Plate>();
        }

        private void OnEnable()
        {
            _plate.onIngredientAdded += OnIngredientAdded;
        }

        private void OnDisable()
        {
            _plate.onIngredientAdded -= OnIngredientAdded;
        }

        private void OnIngredientAdded(object sender, KitchenObjEnum e)
        {
            // var obj = KitchenObjFactory.CreateKitchenObj(e);
            foreach (var enum2Obj in _objEnum2Objs)
            {
                if (enum2Obj.objEnum == e)
                {
                    enum2Obj.obj.SetActive(true);
                }
            }
        }
    }
}