using System;
using Unity.Netcode;
using UnityEngine;

namespace Common
{
    [Serializable]
    public struct GridCell : INetworkSerializable, System.IEquatable<GridCell>
    {
        public bool Busy;
        public Vector2Int Position;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                FastBufferReader reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out Busy);
                reader.ReadValueSafe(out Position);
            }
            else
            {
                FastBufferWriter writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(Busy);
                writer.WriteValueSafe(Position);
            }
        }

        public bool Equals(GridCell other)
        {
            return Busy == other.Busy && Position == other.Position;
        }
    }
}
