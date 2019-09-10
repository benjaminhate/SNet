using System;
using SNet.Core.Common.Serializer;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable]
    public class SNetSerializableObject : ISNetSerialization
    {
        public object internalObject;

        public SNetSerializableObject() { }
        
        public SNetSerializableObject(object internalObject)
        {
            this.internalObject = internalObject;
        }
        
        public byte[] Serialize()
        {
            return NetworkBinary.Serialize(this);
        }

        object ISNetSerialization.Deserialize(byte[] array)
        {
            return Deserialize(array);
        }

        public SNetSerializableObject Deserialize(byte[] array)
        {
            var shift = 0;
            return NetworkBinary.Deserialize<SNetSerializableObject>(array, ref shift);
        }
    }
    
    public class SNetObjectEvent : SNetEvent<SNetSerializableObject>
    {
        [Serializable] public class ObjectEvent : UnityEvent<object>{ }
        public new ObjectEvent clientEventCallbacks;
    }
}