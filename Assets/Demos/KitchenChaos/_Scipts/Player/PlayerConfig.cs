using System;
using Unity.Netcode;

namespace Kitchen
{
    public struct PlayerConfig : IEquatable<PlayerConfig> , INetworkSerializable
    {
        public ulong clientId;
        public int colorId;
        public int swpanPointId;

        public bool Equals(PlayerConfig other)
        {
            return clientId == other.clientId && colorId == other.colorId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref colorId);
        }
    }
}