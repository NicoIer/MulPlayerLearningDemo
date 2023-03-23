using Nico.Design;
using Unity.Netcode;

namespace Nico.Network
{
    /// <summary>
    /// 本地单例模式
    /// 会存在多个实例 但 通过 LocalInstance 获取到的一定是自己客户端对应的实例
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
            base.OnNetworkDespawn();
            if (IsOwner)
            {
                
                LocalInstance = null;
            }
        }
    }
}