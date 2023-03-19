using System;
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
        private static Dictionary<string, RecipeData> _recipeDict;

        [SerializeField] private List<RecipeData> _recipeList;

        private static readonly LinkedList<RecipeData> _waitingQueue = new();
        public static DeliveryManager Singleton { get; private set; }

        //ToDo 后续将所有配置信息保存到一个位置
        public readonly string recipeDataPath =
            "D:/UserData/GitHub/MulPlayerLearningDemo/Assets/Resources/So/Recipe.json";

        private CancellationTokenSource _orderGenerateCts;

        public float spawnTime = 5f;
        public float spawnTimeRange = 2f;
        public int maxOrderCount = 3;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;

            var contentStr = File.ReadAllText(recipeDataPath);
            _recipeDict = JsonConvert.DeserializeObject<Dictionary<string, RecipeData>>(contentStr);
            _recipeList = new List<RecipeData>(_recipeDict.Values);
        }

        private void OnEnable()
        {
            _GenerateOrder().Forget();
        }

        private void OnDisable()
        {
            _orderGenerateCts?.Cancel();
        }

        private bool _isGeneratingOrder = false;

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
                //ToDo 生成订单
                if (_waitingQueue.Count < maxOrderCount)
                {
                    //从食谱中随机选取一个
                    var randomIndex = UnityEngine.Random.Range(0, _recipeDict.Count);
                    var recipeData = _recipeList[randomIndex];
                    _waitingQueue.AddLast(recipeData);
                    Debug.Log($"waiting:{recipeData.recipeName},content:{recipeData.recipe}");
                }
                else
                {
                    //ToDo 通知UI订单已满
                    _isGeneratingOrder = false;
                    return;
                }
            }

            _isGeneratingOrder = false;
        }

        public bool TryDeliverOrder(HashSet<KitchenObjEnum> ingredients)
        {
            //从判断等待队列中是否有对应的订单
            RecipeData? target = null;
            foreach (var order in _waitingQueue)
            {
                if (order.recipe.Length != ingredients.Count)
                    continue;
                //长度相同则一一比对
                bool flag = order.recipe.All(objEnum => ingredients.Contains(objEnum));

                //如果有一个不相同则跳过
                if (!flag) continue;
                //如果有相同则订单完成 跳出检查
                target = order;
                break;
            }

            if (target == null) return false;
            Debug.Log("订单完成!!");
            //完成订单时 
            _waitingQueue.Remove((RecipeData)target);
            //尝试重新启动订单生成任务
            if (!_isGeneratingOrder)
            {
                _GenerateOrder().Forget();
            }

            return true;
        }
    }
}