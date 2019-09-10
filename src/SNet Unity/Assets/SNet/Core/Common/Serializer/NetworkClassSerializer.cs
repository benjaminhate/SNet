using System;
using System.Collections.Generic;

namespace SNet.Core.Common.Serializer
{
    public class NetworkClassSerializer : NetworkSerializer
    {
        /// <summary>
        /// Serialize an object into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>A byte array of the serialized object</returns>
        public static byte[] Serialize<T>(T obj)
        {
            var list = new List<byte>();

            var infos = GetInfos<T>();

            // Loop on fields
            foreach (var info in infos)
            {
                if (Attribute.IsDefined(info, typeof(NotIncluded)))
                {
                    continue;
                }

                var value = info.GetValue(obj);
                var type = info.FieldType;

                var encodeSubType = Attribute.IsDefined(info, typeof(EncodeSubType));
                list.AddRange(TypeSerializer(type, value, encodeSubType));
            }

            return list.ToArray();
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
            var obj = GetDefaultValue(type);
            if (type.IsClass)
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                    throw new InvalidOperationException("The default constructor does not exist for the class : " + type.Name);

                obj = Convert.ChangeType(constructor.Invoke(null), type);
            }

            var infos = GetInfos(type);

            foreach (var info in infos)
            {
                if (Attribute.IsDefined(info, typeof(NotIncluded)))
                {
                    continue;
                }

                var fieldType = info.FieldType;
                var decodeSubType = Attribute.IsDefined(info, typeof(EncodeSubType));
                var value = TypeDeserialize(fieldType, array, ref shift, decodeSubType);

                info.SetValue(obj, value);
            }

            return obj;
        }

        private static object GetDefaultValue(Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}
