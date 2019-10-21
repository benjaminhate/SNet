using System;
using UnityEngine;

namespace SNet.Core.Common.Serializer.UnitySerializer
{
    [Serializable]
    public struct SerializableTransform
    {
        public SerializableVector3 position;
        public SerializableQuaternion rotation;
        public SerializableVector3 scale;
        
        public SerializableTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public static implicit operator SerializableTransform(Transform transform)
        {
            return new SerializableTransform(transform.position, transform.rotation, transform.localScale);
        }
    }
}