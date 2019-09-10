using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SNet.Core.Common.Serializer
{
    public class NetworkListSerializer : NetworkSerializer
    {
        /// <summary>
        /// Serialize an object into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <param name="encodeSubType">Tell if the type of the items in the list must be serialized</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>A byte array of the serialized object</returns>
        public static byte[] Serialize<T>(T obj, bool encodeSubType = false) where T : IList
        {
            var list = new List<byte>();

            var count = obj.Count;
            list.AddRange(GetBytes(count));

            var genType = typeof(T).GetGenericArguments()[0];
            var childTypes = GetAllSubtypes(genType);
            var optiType = GetOptimizedType(childTypes.Count);

            for (var i = 0; i < count; i++)
            {
                var subVal = obj[i];
                var subType = subVal.GetType();
                if (encodeSubType)
                {
                    var encodingIndex = Convert.ChangeType(childTypes.IndexOf(subType), optiType);
                    list.AddRange(GetBytes(encodingIndex));
                }
                list.AddRange(TypeSerializer(subType, subVal));
            }

            return list.ToArray();
        }

        /// <summary>
        /// Deserialize a byte array into an object
        /// </summary>
        /// <param name="array">The byte array</param>
        /// <param name="shift">The shift for the array</param>
        /// <param name="decodeSubType">Tell if the type of the items in the list must be deserialized</param>
        /// <typeparam name="T">the type of the object</typeparam>
        /// <returns>An object of the deserialized byte array</returns>
        public static T Deserialize<T>(byte[] array, ref int shift, bool decodeSubType = false) where T : IList
        {
            return (T) Deserialize(array, ref shift, typeof(T), decodeSubType);
        }

        public static object Deserialize(byte[] array, ref int shift, Type type, bool decodeSubType = false)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
                throw new InvalidOperationException("The default constructor does not exist for the class : " + type.Name);

            var obj = (IList)Convert.ChangeType(constructor.Invoke(null), type);

            var listCount = (int)FromBytes(typeof(int), array, ref shift);
            var subType = type.GetGenericArguments()[0];
            var childTypes = GetAllSubtypes(subType);
            var optiType = GetOptimizedType(childTypes.Count);

            for (var i = 0; i < listCount; i++)
            {
                if (decodeSubType)
                {
                    var decodeIndex = Convert.ToInt32(FromBytes(optiType, array, ref shift));
                    subType = childTypes[decodeIndex];
                }
                obj.Add(TypeDeserialize(subType, array, ref shift));
            }
            return obj;
        }

        /// <summary>
        /// Get all the subtypes of a specific type
        /// </summary>
        /// <param name="type">The type to look for</param>
        /// <returns>A list of subtypes</returns>
        private static List<Type> GetAllSubtypes(Type type)
        {
            var subTypes = Assembly.GetAssembly(type)
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(type))
                .ToList();
            subTypes.Insert(0, type);
            return subTypes;
        }

        /// <summary>
        /// Get the optimized type for a long value
        /// </summary>
        /// <param name="value">The value to optimize</param>
        /// <returns>The optimized type</returns>
        private static Type GetOptimizedType(long value)
        {
            if (value < byte.MaxValue)
                return typeof(byte);
            if (value < short.MaxValue)
                return typeof(short);
            if (value < int.MaxValue)
                return typeof(int);
            return typeof(long);
        }
    }
}
