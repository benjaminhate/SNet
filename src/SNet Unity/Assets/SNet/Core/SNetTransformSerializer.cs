using SNet.Core.Common.Serializer;
using SNet.Core.Common.Serializer.UnitySerializer;
using UnityEngine;

namespace SNet.Core
{
    public static class SNetTransformSerializer
    {
        public static byte[] Serialize(Transform transform)
        {
            SerializableTransform serializableTransform = transform;
            return NetworkBinary.Serialize(serializableTransform);
        }

        public static void Deserialize(byte[] array, out Vector3 position, out Quaternion rotation, out Vector3 scale)
        {
            var shift = 0;
            var serializableTransform = NetworkBinary.Deserialize<SerializableTransform>(array, ref shift);
            position = serializableTransform.position;
            rotation = serializableTransform.rotation;
            scale = serializableTransform.scale;
        }
    }
}