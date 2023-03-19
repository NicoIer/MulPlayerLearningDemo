using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen.UI
{
    public class PlateIconsUI : MonoBehaviour
    {
        public GameObject iconPrefab;
        private Plate _plate;
        private readonly List<KitchenObjIcon> _icons = new List<KitchenObjIcon>();

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
            var obj = Instantiate(iconPrefab, transform);
            var icon = obj.GetComponent<KitchenObjIcon>();
            _icons.Add(icon);
            //修改Icon图标
            var dataSo = DataTableManager.Sigleton.GetKitchenObjSo(e);
            icon.SetData(dataSo);
        }

        private void OnIngredientRemoved(object sender, KitchenObjEnum e)
        {
            //找到第一个对应的Icon 移除掉
            KitchenObjIcon target = null;
            foreach (var icon in _icons)
            {
                if (icon.objEnum == e)
                {
                    target = icon;
                    break;
                }
            }

            if (target == null)
            {
                return;
            }

            _icons.Remove(target);
            //ToDO 可以优化 不真正的destroy
            Destroy(target.gameObject);
        }
    }
}