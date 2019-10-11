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
        public SNetHash128 AssetId;
        public SNetSceneId SceneId;
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
            SceneId = idMsg.SceneId;
            Position = idMsg.Position;
            Rotation = idMsg.Rotation;
        }
    }

    public class EmptyMessage : MessageBase
    {
        public override byte[] Serialize()
        {
            return new byte[0];
        }

        public override void Deserialize(byte[] array)
        {
            
        }
    }

    public class ReadyMessage : EmptyMessage
    {
        
    }
}