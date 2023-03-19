using System;
using UnityEngine;

namespace Kitchen
{
    [Serializable]
    public struct RecipeData
    {
        public string recipeName;
        public KitchenObjEnum[] recipe;
    }
}