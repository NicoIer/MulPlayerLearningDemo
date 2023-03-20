using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Nico.DesignPattern
{
    /// <summary>
    /// 基于MonoBehaviour的单例模式 仅场景内单例 不会跨场景 切换场景会被销毁
    /// 这个单例是线程安全的 
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        private static readonly object _lock = typeof(T);
        private static T _instance;
        private static bool _applicationQuit;

        public static T Instance
        {
            get
            {
                if (_applicationQuit)
                {
                    return null;
                }
                if (_instance == null)
                {
                    //如果时在 OnDisable时访问 则会返回null
                    if (Application.isPlaying == false)
                    {
                        return null;
                    }

                    //双重检查锁
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = FindObjectOfType<T>(); //从场景中寻找一个T类型的组件
                            if (_instance == null)
                            {
                                //找不见 就 new 一个
                                Debug.Log($"can not find {typeof(T).Name} auto create one");
                                GameObject obj = new GameObject(typeof(T).Name);
                                _instance = obj.AddComponent<T>();
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            _applicationQuit = false;
            //如果Awake前没有被访问 那么就会在Awake中初始化
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                //如果已经被访问过了 代表已经有一个对应的单例对象存在了 那么就会在Awake中销毁自己
                Destroy(gameObject);
                return;
            }
        }

        protected void OnApplicationQuit()
        {
            _applicationQuit = true;
        }

        protected virtual void OnDestroy()
        {
            //当单例对象被销毁的时候 会将_instance设置为null
            //如何保证单例对象是最后被销毁的呢
            _instance = null;
        }
    }
}