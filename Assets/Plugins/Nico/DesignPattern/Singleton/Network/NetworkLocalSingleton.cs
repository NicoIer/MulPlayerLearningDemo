﻿using Unity.Netcode;
using UnityEngine;

namespace Nico.DesignPattern.Singleton.Network
{
    /// <summary>
    /// 本地单例模式
    /// 仅Owner能够访问的单例对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NetworkLocalSingleton<T> : NetworkBehaviour, ISingleton where T : NetworkLocalSingleton<T>
    {
        public static T LocalInstance { get; private set; }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                LocalInstance = this as T;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                
                LocalInstance = null;
            }
        }
    }
}