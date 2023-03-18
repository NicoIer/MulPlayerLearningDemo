using System;
using System.Collections.Generic;
using Kitchen;
using Mono.CSharp;
using UnityEngine;

namespace Nico
{
    public class DataTableManager : MonoBehaviour
    {
        public static DataTableManager Sigleton { get; private set; }
        [field: SerializeField] private List<KitchenObjSo> kitchenObjSoList = new();
        private Dictionary<KitchenObjEnum, KitchenObjSo> _kitchenDict;

        public KitchenObjSo Get(KitchenObjEnum kitchenObjEnum)
        {
            if (_kitchenDict == null)
            {
                _kitchenDict = new();
                foreach (var kitchenObjSo in kitchenObjSoList)
                {
                    _kitchenDict.TryAdd(kitchenObjSo.kitchenObjEnum, kitchenObjSo);
                }
            }

            return _kitchenDict[kitchenObjEnum];
        }


        private void Awake()
        {
            if (Sigleton != null)
            {
                Destroy(gameObject);
                return;
            }

            Sigleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}