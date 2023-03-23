using System;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

namespace Nico.DesignPattern.Singleton.Network
{
    /// <summary>
    /// 网络Mono单例模式
    /// 游戏中只需要一个样的对象
    /// 仅场景内单例 不会跨场景 切换场景会被销毁
    /// 这个单例是线程安全的
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    
    public class NetworkMonoSingleton<T> : NetworkBehaviour, ISingleton where T : NetworkMonoSingleton<T>
    {
        private static T _instance;
        private static object _lock = typeof(T);

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //双重检查锁
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = FindObjectOfType<T>(); //从场景中寻找一个T类型的组件
                            if (_instance == null)
                            {
                                //找不见 就 new 一个
                                GameObject obj = new GameObject(typeof(T).Name);
                                _instance = obj.AddComponent<T>();
                            }
                        }
                    }
                }

                return _instance;
            }
        }
        
        [CanBeNull]
        public static T GetInstanceUnSafe(bool throwError = false)
        {
            if (_instance == null)
            {
                if (throwError)
                {
                    throw new Exception("Application is quitting !!!!");
                }

                return null;
            }

            return Instance;
        }
        protected virtual void Awake()
        {
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

        protected virtual void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            //当单例对象被销毁的时候 会将_instance设置为null
            //如何保证单例对象是最后被销毁的呢
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}