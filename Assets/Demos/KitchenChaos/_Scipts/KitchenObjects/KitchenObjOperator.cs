﻿using System.Collections.Generic;
using Kitchen;
using UnityEngine;

namespace Kitchen
{
    public static class KitchenObjOperator
    {
        
        public static void SpawnKitchenObj(KitchenObjEnum objEnum, ICanHoldKitchenObj holder)
        {
            KitchenObjFactory.Instance.CreateKitchenObj(objEnum, holder);
            
        }

        public static void ExchangeKitchenObj(ICanHoldKitchenObj holder1, ICanHoldKitchenObj holder2)
        {
            //不同的情况 1.两个都有物体 2.一个有一个没有 3.都没有
            bool h1 = holder1.HasKitchenObj();
            bool h2 = holder2.HasKitchenObj();
            if (!h1 && !h2) return;


            //1.两个都有物体
            if (h1 && h2)
            {
                var obj1 = holder1.GetKitchenObj();
                var obj2 = holder2.GetKitchenObj();
                holder1.SetKitchenObj(obj2);
                obj2.SetHolder(holder1);
                holder2.SetKitchenObj(obj1);
                obj1.SetHolder(holder2);
                return;
            }

            //2.一个有一个没有
            if (h1)
            {
                var obj1 = holder1.GetKitchenObj();
                holder1.ClearKitchenObj();
                holder2.SetKitchenObj(obj1);
                obj1.SetHolder(holder2);
                return;
            }

            {
                var obj2 = holder2.GetKitchenObj();
                holder2.ClearKitchenObj();
                holder1.SetKitchenObj(obj2);
                obj2.SetHolder(holder1);
            }
        }

        public static void PutKitchenObj(ICanHoldKitchenObj putter, ICanHoldKitchenObj reciever)
        {
            var obj = putter.GetKitchenObj();
            obj.SetHolder(reciever);
            reciever.SetKitchenObj(obj);
            putter.ClearKitchenObj();
        }

        private static readonly HashSet<KitchenObjEnum> _cookableKitchenObjEnumSet = new()
        {
            KitchenObjEnum.MeatPattyUncooked,
            KitchenObjEnum.MeatPattyCooked
        };

        public static bool CanCook(KitchenObj kitchenObj)
        {
            return _cookableKitchenObjEnumSet.Contains(kitchenObj.objEnum);
        }

        public static void Cook(KitchenObj beCookedObj, ICanHoldKitchenObj holder)
        {
            var cookedObjSo = DataTableManager.Sigleton.GetCookedKitchenObjSo(beCookedObj.objEnum);
            beCookedObj.DestroySelf();
            SpawnKitchenObj(cookedObjSo.kitchenObjEnum, holder);
        }

        public static bool WillBeBurned(KitchenObjEnum kitchenObjEnum)
        {
            //ToDO
            if (kitchenObjEnum == KitchenObjEnum.MeatPattyCooked)
            {
                return true;
            }

            return false;
        }

        public static void PutToPlate(KitchenObj kitchenObj, Plate plate)
        {
            //尝试将物体放入盘子
            if (plate.TryAddIngredient(kitchenObj))
            {
                //销毁掉物体
                kitchenObj.DestroySelf();
            }
        }
    }
}