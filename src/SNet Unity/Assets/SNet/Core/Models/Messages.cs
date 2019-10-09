using SNet.Core.Common.Serializer;
using SNet.Core.Common.Serializer.UnitySerializer;

namespace SNet.Core.Models
{
    public abstract class MessageBase
    {
        public abstract byte[] Serialize();

        public abstract void Deserialize(byte[] array);
    }

    public class ObjectSpawnMessage : MessageBase
    {
        public string Id;
        public NetworkHash128 AssetId;
        public SerializableVector3 Position;
        public SerializableQuaternion Rotation;
        
        public override byte[] Serialize()
        {
            return NetworkBinary.Serialize(this);
        }

        public override void Deserialize(byte[] array)
        {
            var shift = 0;
            var idMsg = NetworkBinary.Deserialize<ObjectSpawnMessage>(array, ref shift);
            Id = idMsg.Id;
            AssetId = idMsg.AssetId;
            Position = idMsg.Position;
            Rotation = idMsg.Rotation;
        }
    }
}