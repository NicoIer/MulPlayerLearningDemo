using System;
using UnityEngine;

namespace Kitchen
{
    
    [CreateAssetMenu(fileName = "KitchenObjSo", menuName = "ScriptableObjects/KitchenObjSo", order = 0)]
    public class KitchenObjSo : ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
        public KitchenObjEnum kitchenObjEnum;
    }
    
}