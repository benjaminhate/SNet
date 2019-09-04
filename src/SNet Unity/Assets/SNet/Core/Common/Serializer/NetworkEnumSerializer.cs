using System;

namespace SNet.Core.Common.Serializer
{
    public class NetworkEnumSerializer : NetworkSerializer
    {
        /// <summary>
        /// Serialize an object into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <param name="cast">The type of the enum</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>A byte array of the serialized object</returns>
        public static byte[] Serialize<T>(T obj, Type cast)
        {
            return NetworkPrimitiveSerializer.Serialize(Convert.ChangeType(obj, cast));
        }

        /// <summary>
        /// Deserialize a byte array into a typed object
        /// </summary>
        /// <param name="array">The byte array</param>
        /// <param name="shift">The shift for the array</param>
        /// <param name="cast">The type of the enum</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>A typed object of the deserialized byte array</returns>
        public static T Deserialize<T>(byte[] array, ref int shift, Type cast)
        {
            return (T) Deserialize(array, ref shift, cast, typeof(T));
        }

        /// <summary>
        /// Deserialize a byte array into an object
        /// </summary>
        /// <param name="array">The byte array</param>
        /// <param name="shift">The shift for the array</param>
        /// <param name="cast">The type of the enum</param>
        /// <param name="type">The type of the object</param>
        /// <returns>An object of the deserialized byte array</returns>
        public static object Deserialize(byte[] array, ref int shift, Type cast, Type type)
        {
            var val = NetworkPrimitiveSerializer.Deserialize(array, ref shift, cast);
            return Enum.ToObject(type, val);
        }
    }
}
