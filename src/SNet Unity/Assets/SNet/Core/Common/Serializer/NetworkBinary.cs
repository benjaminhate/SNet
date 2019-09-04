using System;

namespace SNet.Core.Common.Serializer
{
    public static class NetworkBinary
    {
        /// <summary>
        /// Serialize an object into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>A byte array of the serialized object</returns>
        public static byte[] Serialize<T>(T obj)
        {
            var t = typeof(T);
            if (t.IsPrimitive)
                return NetworkPrimitiveSerializer.Serialize(obj);
            if (t == typeof(string))
                return NetworkStringSerializer.Serialize((string)(object)obj);
            if (t.IsEnum)
                return NetworkEnumSerializer.Serialize(obj, typeof(byte));
            return NetworkClassSerializer.Serialize(obj);
        }

        /// <summary>
        /// Deserialize a byte array into a typed object
        /// </summary>
        /// <param name="array">The byte array</param>
        /// <param name="shift">The shift for the array</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>A typed object of the deserialized byte array</returns>
        public static T Deserialize<T>(byte[] array, ref int shift)
        {
            return (T) Deserialize(array, ref shift, typeof(T));
        }

        /// <summary>
        /// Deserialize a byte array into an object
        /// </summary>
        /// <param name="array">The byte array</param>
        /// <param name="shift">The shift for the array</param>
        /// <param name="type">The type of the object</param>
        /// <returns>An object of the deserialized byte array</returns>
        public static object Deserialize(byte[] array, ref int shift, Type type)
        {
            if (type.IsPrimitive)
                return NetworkPrimitiveSerializer.Deserialize(array, ref shift, type);
            if (type == typeof(string))
                return Convert.ChangeType(NetworkStringSerializer.Deserialize(array, ref shift), type);
            if (type.IsEnum)
                return NetworkEnumSerializer.Deserialize(array, ref shift, typeof(byte), type);
            return NetworkClassSerializer.Deserialize(array, ref shift, type);
        }
    }
}
