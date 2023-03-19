using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen.UI
{
    public class RecipeIcon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Transform iconContainer;
        [SerializeField] private GameObject iconPrefab;
        private List<GameObject> _icons = new();

        private void Awake()
        {
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetRecipe(RecipeData recipeData)
        {
            _textMeshPro.text = recipeData.recipeName;
            for (int i = 0; i < recipeData.recipe.Length; i++)
            {
                var objEnum = recipeData.recipe[i];
                var so = DataTableManager.Sigleton.GetKitchenObjSo(objEnum);
                //如果已经有合适的icon 则不再生成
                if (i < _icons.Count)
                {
                    _icons[i].gameObject.SetActive(true);
                    _icons[i].GetComponent<Image>().sprite = so.sprite;
                    continue;
                }
                //不够则生成
                var icon = Instantiate(iconPrefab, iconContainer);
                icon.GetComponent<Image>().sprite = so.sprite;
                _icons.Add(icon);
            }
            //将多余的icon隐藏
            for (int i = recipeData.recipe.Length; i < _icons.Count; i++)
            {
                _icons[i].gameObject.SetActive(false);
            }
        }
    }
}