using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nico
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefab;
        private readonly Queue<GameObject> _pool = new();

        public GameObject Get()
        {
            if (_pool.Count == 0)
            {
                GameObject go = Instantiate(prefab);
                go.SetActive(true);
                return go;
            }

            var obj =  _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        public void Return(GameObject go)
        {
            go.transform.SetParent(transform);
            go.SetActive(false);
            _pool.Enqueue(go);
        }
    }
}