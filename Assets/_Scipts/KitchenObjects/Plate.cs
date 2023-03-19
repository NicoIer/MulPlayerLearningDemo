using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class Plate : KitchenObj
    {
        private static readonly HashSet<KitchenObjEnum> _cannotPlace = new()
        {
            KitchenObjEnum.Plate,
        };

        public EventHandler<KitchenObjEnum> onIngredientAdded;
        private readonly HashSet<KitchenObjEnum> _ingredients = new();

        public bool TryAddIngredient(KitchenObj obj)
        {
            if (_cannotPlace.Contains(obj.objEnum))
            {
                return false;
            }
            if(_ingredients.Contains(obj.objEnum))
                return false;
            //ToDO 下面判断食谱是否正确 但是目前没有食谱 所以先不判断
            //只有食谱正确才可以放置
            
            _ingredients.Add(obj.objEnum);
            //将对应物品的位置设置到盘子上
            onIngredientAdded?.Invoke(this, obj.objEnum);

            return true;
        }
    }
}