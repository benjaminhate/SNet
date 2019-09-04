using System;

namespace SNet.Core.Common.Serializer
{
    public class NetworkPrimitiveSerializer : NetworkSerializer
    {
        /// <summary>
        /// Serialize an object into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>A byte array of the serialized object</returns>
        public static byte[] Serialize<T>(T obj)
        {
            return GetBytes(obj);
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
            return FromBytes(type, array, ref shift);
        }
    }
}
