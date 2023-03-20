using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kitchen.UI
{
    public class RecipeManagerUI : MonoBehaviour
    {
        public Transform recipeUIContainer;
        public GameObject recipeUIPrefab;
        public List<RecipeIcon> recipeIcons = new();

        private void Start()
        {
            _SubscribeEvent();
        }


        private bool _listening;

        private void _SubscribeEvent()
        {
            try
            {
                if (!_listening)
                {
                    var deliveryManager = DeliveryManager.Instance;
                    deliveryManager.OnOrderFinished += _OnOrderFinished;
                    deliveryManager.OnOrderAdded += _OnOrderAdded;
                    deliveryManager.OnOrderSuccess += _OnOrderSuccess;
                    deliveryManager.OnOrderFailed += _OnOrderFailed;
                    _listening = true;
                }
            }
            catch (NullReferenceException)
            {
                //由于不同对象的OnEnable调用顺序不同，可能会出现DeliveryManager还没有初始化的情况
                //因为OnEnable会在Awake之前调用，所以可能会出现DeliveryManager还没有初始化的情况
            }
        }


        private void OnEnable()
        {
            _SubscribeEvent();
        }

        private void OnDisable()
        {
            try
            {
                DeliveryManager.Instance.OnOrderFinished -= _OnOrderFinished;
                DeliveryManager.Instance.OnOrderAdded -= _OnOrderAdded;
                DeliveryManager.Instance.OnOrderSuccess -= _OnOrderSuccess;
                DeliveryManager.Instance.OnOrderFailed -= _OnOrderFailed;
                _listening = false;
            }
            catch (NullReferenceException)
            {
            }
        }


        private void _OnOrderFinished(object sender, RecipeData e)
        {
            SetWaitingRecipes(DeliveryManager.Instance.GetWaitingQueue());
        }

        private void _OnOrderSuccess(object sender, Vector3 e)
        {
            SetWaitingRecipes(DeliveryManager.Instance.GetWaitingQueue());
        }

        private void _OnOrderFailed(object sender, Vector3 e)
        {
            SetWaitingRecipes(DeliveryManager.Instance.GetWaitingQueue());
        }

        private void _OnOrderAdded(object sender, RecipeData e)
        {
            SetWaitingRecipes(DeliveryManager.Instance.GetWaitingQueue());
        }

        public void SetWaitingRecipes(ICollection<RecipeData> recipes)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                var recipe = recipes.ElementAt(i);
                //判断是否已经有一个UI可以存放数据
                if (i < recipeIcons.Count)
                {
                    recipeIcons[i].gameObject.SetActive(true);
                    recipeIcons[i].SetRecipe(recipe);
                    continue;
                }

                var recipeUI = Instantiate(recipeUIPrefab, recipeUIContainer).GetComponent<RecipeIcon>();
                recipeIcons.Add(recipeUI);
                recipeUI.SetRecipe(recipe);
            }

            //将多余的UI隐藏
            for (int i = recipes.Count; i < recipeIcons.Count; i++)
            {
                recipeIcons[i].gameObject.SetActive(false);
            }
        }
    }
}