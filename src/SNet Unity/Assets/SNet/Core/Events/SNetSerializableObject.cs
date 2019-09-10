using System;
using SNet.Core.Common.Serializer;

namespace SNet.Core.Events
{
    [Serializable]
    public class SNetSerializableObject : ISNetSerialization, ISNetConvertible<object>
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

        public object ConvertTo()
        {
            return internalObject;
        }

        public object ConvertFrom(object obj)
        {
            return new SNetSerializableObject(obj);
        }
    }
}