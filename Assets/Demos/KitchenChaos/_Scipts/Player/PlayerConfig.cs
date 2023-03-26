using System;
using Unity.Netcode;

namespace Kitchen
{
    public struct PlayerConfig : IEquatable<PlayerConfig> , INetworkSerializable
    {
        public ulong clientId;
        public bool Equals(PlayerConfig other)
        {
            return clientId == other.clientId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
        }
    }
}