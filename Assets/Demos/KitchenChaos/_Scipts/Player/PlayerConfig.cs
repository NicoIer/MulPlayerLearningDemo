using System;
using Unity.Collections;
using Unity.Netcode;

namespace Kitchen
{
    public struct PlayerConfig : IEquatable<PlayerConfig>, INetworkSerializable
    {
        public ulong clientId;
        public int colorId;
        public int spawnPointId;
        public FixedString64Bytes playerName;

        public bool Equals(PlayerConfig other)
        {
            return clientId == other.clientId && colorId == other.colorId && playerName == other.playerName;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref colorId);
            serializer.SerializeValue(ref playerName);
        }
    }
}