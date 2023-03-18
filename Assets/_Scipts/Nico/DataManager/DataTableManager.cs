using System;
using System.Collections.Generic;
using System.IO;
using Nico;
using Mono.CSharp;
using UnityEngine;

namespace Nico
{
    public class DataTableManager : MonoBehaviour
    {
        public static DataTableManager Sigleton { get; private set; }
        private Dictionary<KitchenObjEnum, KitchenObjSo> _kitchenDict;
        private Dictionary<KitchenObjEnum, KitchenObjEnum> _cuttingDict;
        private Dictionary<KitchenObjEnum, int> _cuttingCountDict;

        private const string _kitchenDataSoDir = "So/KitchenObj/";
        private const string _exchangeDictTextPath = "So/KitchenObj/CuttingDict";
        private const string _cuttingCountDictTextPath = "So/KitchenObj/CuttingCountDict";

        public KitchenObjSo GetKitchenObjSo(KitchenObjEnum kitchenObjEnum)
        {
            return _kitchenDict[kitchenObjEnum];
        }

        public KitchenObjSo GetCutKitchenObjSo(KitchenObjEnum kitchenObjEnum)
        {
            try
            {
                return _kitchenDict[_cuttingDict[kitchenObjEnum]];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetCuttingCount(KitchenObjEnum kitchenObjEnum)
        {
            return _cuttingCountDict[kitchenObjEnum];
        }

        private void _Init()
        {
            _InitKitchenDict();
            _InitCuttingChangeDict();
            _InitCuttingCountDict();
        }

        private void _InitCuttingChangeDict()
        {
            //从指定位置读取文本文件 并解析成Dictionary<KitchenObjEnum, KitchenObjEnum>字典
            //ToDo 当前使用的是 Resources 后期考虑换成 AssetBundle 或者 Addressable
            var exchangeDictText = Resources.Load<TextAsset>(_exchangeDictTextPath);

            var exchangeDictTextStr = exchangeDictText.text;
            var exchangeDictTextStrList = exchangeDictTextStr.Split('\n');
            _cuttingDict = new();
            foreach (var exchangeDictTextStrLine in exchangeDictTextStrList)
            {
                var exchangeDictTextStrLineList = exchangeDictTextStrLine.Split(',');
                var key = (KitchenObjEnum)System.Enum.Parse(typeof(KitchenObjEnum), exchangeDictTextStrLineList[0]);
                var value = (KitchenObjEnum)System.Enum.Parse(typeof(KitchenObjEnum), exchangeDictTextStrLineList[1]);
                _cuttingDict.TryAdd(key, value);
            }
        }

        private void _InitCuttingCountDict()
        {
            //从指定位置读取文本文件 并解析成Dictionary<KitchenObjEnum, int>字典
            //ToDo 当前使用的是 Resources 后期考虑换成 AssetBundle 或者 Addressable
            var cuttingCountDictText = Resources.Load<TextAsset>(_cuttingCountDictTextPath);
            var cuttingCountDictTextStr = cuttingCountDictText.text;
            var cuttingCountDictTextStrList = cuttingCountDictTextStr.Split('\n');
            _cuttingCountDict = new();

            foreach (var line in cuttingCountDictTextStrList)
            {
                var kv = line.Split(',');
                var key = (KitchenObjEnum)System.Enum.Parse(typeof(KitchenObjEnum), kv[0]);
                var value = int.Parse(kv[1]);
                _cuttingCountDict.TryAdd(key, value);
            }
        }

        private void _InitKitchenDict()
        {
            _kitchenDict = new();
            //遍历指定目录 kitchenDataSoDir 读取其下所有的KitchenObjSo到Dict中
            //ToDo 当前使用的是 Resources 后期考虑换成 AssetBundle 或者 Addressable
            var kitchenObjSoList = Resources.LoadAll<KitchenObjSo>(_kitchenDataSoDir);
            foreach (var kitchenObjSo in kitchenObjSoList)
            {
                _kitchenDict.TryAdd(kitchenObjSo.kitchenObjEnum, kitchenObjSo);
            }
        }

        private void Awake()
        {
            if (Sigleton != null)
            {
                Destroy(gameObject);
                return;
            }

            Sigleton = this;
            _Init();
            DontDestroyOnLoad(gameObject);
        }
    }
}