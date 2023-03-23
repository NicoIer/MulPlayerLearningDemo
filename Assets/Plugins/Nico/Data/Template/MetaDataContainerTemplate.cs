using System.Collections.Generic;
using UnityEngine;

namespace Nico.Util
{
     public class MetaDataContainer : ScriptableObject,IMetaDataContainer
     {
         // protected HashSet<int> metaSet = new HashSet<int>();
         // [SerializeField] protected List<MetaData> metaList = new List<MetaData>();
         //
         // public void AddData(MetaData metaData)
    //     // {
    //     //     if (metaSet.Contains(metaData.id))
    //     //     {
    //     //         Debug.LogWarning($"{metaData}->重复的id:{metaData.id}");
    //     //         return;
    //     //     }
    //     //
    //     //     metaSet.Add(metaData.id);
    //     //     metaList.Add(metaData);
    //     // }
    //
    //     public void AddData<T>(IMetaData metaData) where T : IMetaData
    //     {
    //         // var x = (T) metaData;
    //     }
     }
}