﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Collections;
using UnityEngine;

namespace Kitchen
{
    public class DeliveryManager : MonoBehaviour
    {
        private static DeliveryManager _singleton;

        public static DeliveryManager Singleton => _singleton;

        //ToDo 后续将所有配置信息保存到一个位置
        private const string _recipeDataPath =
            "D:/UserData/GitHub/MulPlayerLearningDemo/Assets/Resources/So/Recipe.json";

        private Dictionary<string, RecipeData> _recipeDict;
        private readonly LinkedList<RecipeData> _waitingQueue = new();

        [SerializeField] private List<RecipeData> recipeList;
        private CancellationTokenSource _orderGenerateCts;

        public float spawnTime = 5f;
        public float spawnTimeRange = 2f;
        public int maxOrderCount = 3;
        private bool _isGeneratingOrder = false;
        public event EventHandler<RecipeData> OnOrderAdded;
        public event EventHandler<RecipeData> OnOrderFinished;

        private void Awake()
        {
            if (_singleton != null)
            {
                throw new Exception("DeliveryManager already exists");
            }

            _singleton = this;

            var contentStr = File.ReadAllText(_recipeDataPath);
            _recipeDict = JsonConvert.DeserializeObject<Dictionary<string, RecipeData>>(contentStr);
            if (_recipeDict == null)
            {
                throw new NullReferenceException();
            }

            recipeList = new List<RecipeData>(_recipeDict.Values);
        }

        private void OnEnable()
        {
            _GenerateOrder().Forget();
        }

        private void OnDisable()
        {
            _orderGenerateCts?.Cancel();
        }

        private void OnDestroy()
        {
            _singleton = null;
        }

        private async UniTask _GenerateOrder()
        {
            _isGeneratingOrder = true;
            _orderGenerateCts = new CancellationTokenSource();
            while (!_orderGenerateCts.IsCancellationRequested)
            {
                //等待指定时间
                var randomFloat = UnityEngine.Random.Range(-spawnTimeRange, spawnTimeRange);
                await UniTask.Delay(TimeSpan.FromSeconds(spawnTime + randomFloat),
                    cancellationToken: _orderGenerateCts.Token);
                if (_waitingQueue.Count < maxOrderCount)
                {
                    //从食谱中随机选取一个
                    var randomIndex = UnityEngine.Random.Range(0, _recipeDict.Count);
                    var recipeData = recipeList[randomIndex];
                    _waitingQueue.AddLast(recipeData);
                    OnOrderAdded?.Invoke(this, recipeData);
                }
                else
                {
                    //订单已满
                    _isGeneratingOrder = false;
                    return;
                }
            }

            _isGeneratingOrder = false;
        }

        public bool TryDeliverOrder(HashSet<KitchenObjEnum> ingredients)
        {
            //检查是否有匹配的订单
            RecipeData? target = _CheckIngredients(ingredients);
            if (target == null) return false;
            Debug.Log("订单完成!!");
            //完成订单时 
            _waitingQueue.Remove((RecipeData)target);
            OnOrderFinished?.Invoke(this, (RecipeData)target);
            //尝试重新启动订单生成任务
            if (!_isGeneratingOrder)
            {
                _GenerateOrder().Forget();
            }

            return true;
        }

        private RecipeData? _CheckIngredients(HashSet<KitchenObjEnum> ingredients)
        {
            RecipeData? target = null;
            foreach (var order in _waitingQueue)
            {
                if (order.recipe.Length != ingredients.Count)
                    continue;
                //检查是否有相同的食材
                Debug.Log($"{string.Join(",", order.recipe)} VS {string.Join(",", ingredients)}");
                bool flag = order.recipe.All(ingredients.Contains);

                //如果有一个不相同则跳过
                if (!flag) continue;
                //如果有相同则订单完成 跳出检查
                target = order;
                break;
            }

            return target;
        }

        public ICollection<RecipeData> GetWaitingQueue()
        {
            return _waitingQueue;
        }
    }
}