using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SNet.Core.Common.Extensions;

namespace SNet.Core.Common.Serializer
{
    public abstract class NetworkSerializer
    {
        /// <summary>
        /// Get the field infos from a specific type
        /// The default flags are : Public Static NonPublic Instance
        /// You can add more flags with the parameter moreFlags
        /// </summary>
        /// <param name="moreFlags">Flags to add to the search</param>
        /// <typeparam name="T">The type to search for</typeparam>
        /// <returns>A list of FieldInfo about the type given</returns>
        protected static IEnumerable<FieldInfo> GetInfos<T>(BindingFlags moreFlags = BindingFlags.Default)
        {
            return GetInfos(typeof(T), moreFlags);
        }

        protected static IEnumerable<FieldInfo> GetInfos(Type type, BindingFlags moreFlags = BindingFlags.Default)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | moreFlags);
        }

        #region Bytes Methods
        /// <summary>
        /// Get the byte array from an object depending on its type
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A byte array representing the object</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the type of the object is not managed</exception>
        protected static byte[] GetBytes(object obj)
        {
            var type = obj.GetType();
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    var valBool = Convert.ToBoolean(obj);
                    return BitConverter.GetBytes(valBool);
                case TypeCode.Byte:
                    var valByte = Convert.ToByte(obj);
                    return new[] { valByte };
                case TypeCode.Char:
                    var valChar = Convert.ToChar(obj);
                    return BitConverter.GetBytes(valChar);
                case TypeCode.Double:
                    var valDou = Convert.ToDouble(obj);
                    return BitConverter.GetBytes(valDou);
                case TypeCode.Int16:
                    var valSho = Convert.ToInt16(obj);
                    return BitConverter.GetBytes(valSho);
                case TypeCode.Int32:
                    var valInt = Convert.ToInt32(obj);
                    return BitConverter.GetBytes(valInt);
                case TypeCode.Int64:
                    var valLon = Convert.ToInt64(obj);
                    return BitConverter.GetBytes(valLon);
                case TypeCode.Single:
                    var valFlo = Convert.ToSingle(obj);
                    return BitConverter.GetBytes(valFlo);
                case TypeCode.UInt16:
                    var valUSh = Convert.ToUInt16(obj);
                    return BitConverter.GetBytes(valUSh);
                case TypeCode.UInt32:
                    var valUIn = Convert.ToUInt32(obj);
                    return BitConverter.GetBytes(valUIn);
                case TypeCode.UInt64:
                    var valULo = Convert.ToUInt64(obj);
                    return BitConverter.GetBytes(valULo);
                case TypeCode.DateTime:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.Decimal:
                    break;
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.SByte:
                    break;
                case TypeCode.String:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new byte[] { 0 };
        }

        /// <summary>
        /// Get an object from a byte array
        /// </summary>
        /// <param name="type">The type of the object</param>
        /// <param name="array">The byte array to decode from</param>
        /// <param name="shift">The shift to apply to the array</param>
        /// <returns>A decoded object</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the type of the object is not managed</exception>
        protected static object FromBytes(Type type, byte[] array, ref int shift)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return BitExtensions.ToBool(array, ref shift);
                case TypeCode.Byte:
                    return BitExtensions.ToByte(array, ref shift);
                case TypeCode.Char:
                    return BitExtensions.ToChar(array, ref shift);
                case TypeCode.Double:
                    return BitExtensions.ToDouble(array, ref shift);
                case TypeCode.Int16:
                    return BitExtensions.ToShort(array, ref shift);
                case TypeCode.Int32:
                    return BitExtensions.ToInt(array, ref shift);
                case TypeCode.Int64:
                    return BitExtensions.ToLong(array, ref shift);
                case TypeCode.Single:
                    return BitExtensions.ToFloat(array, ref shift);
                case TypeCode.UInt16:
                    return BitExtensions.ToUShort(array, ref shift);
                case TypeCode.UInt32:
                    return BitExtensions.ToUInt(array, ref shift);
                case TypeCode.UInt64:
                    return BitExtensions.ToULong(array, ref shift);
                case TypeCode.DateTime:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.Decimal:
                    break;
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.SByte:
                    break;
                case TypeCode.String:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }
        #endregion

        #region Method Invoker
        /// <summary>
        /// Invoke the Deserialize method from the type given and return the value returned
        /// </summary>
        /// <param name="type">The type to call the method</param>
        /// <param name="genericType">The type to feed the generic type</param>
        /// <param name="args">The arguments of the method invoked</param>
        /// <returns>The object returned by the Deserialize method</returns>
        protected static object InvokeDeserialize(Type type, Type genericType, object[] args)
        {
            return InvokeMethod("Deserialize", type, genericType, null, args);
        }

        /// <summary>
        /// Invoke the Serialize method from the type given and return the value returned
        /// </summary>
        /// <param name="type">The type to call the method</param>
        /// <param name="genericType">The type to feed the generic type</param>
        /// <param name="args">The arguments of the method invoked</param>
        /// <returns>The byte array returned by the Serialize method</returns>
        private static IEnumerable<byte> InvokeSerialize(Type type, Type genericType, object[] args)
        {
            return (byte[])InvokeMethod("Serialize", type, genericType, null, args);
        }

        /// <summary>
        /// Invoke a generic method with a given name from a given type
        /// </summary>
        /// <param name="name">The name of the method to invoke</param>
        /// <param name="type">The type to call the method</param>
        /// <param name="genericType">The type to feed the generic type</param>
        /// <param name="obj">The object on which call the method</param>
        /// <param name="args">The arguments of the method invoked</param>
        /// <returns>The object returned by the method</returns>
        private static object InvokeMethod(string name, Type type, Type genericType, object obj, object[] args)
        {
            return type
                .GetMethod(name)
                ?.MakeGenericMethod(genericType)
                .Invoke(obj, args);
        }
        #endregion

        /// <summary>
        /// Serialize an object depending on its type
        /// </summary>
        /// <param name="type">The type of the object to serialize</param>
        /// <param name="value">The object to serialize</param>
        /// <param name="encodeSubType">Optional parameter; Used for ListSerializer</param>
        /// <returns>The byte array representing the serialized object</returns>
        protected static IEnumerable<byte> TypeSerializer(Type type, object value, bool encodeSubType = false)
        {
            if (type.IsPrimitive)
                return InvokeSerialize(typeof(NetworkPrimitiveSerializer), type, new[] { value });
            else if (type == typeof(string))
                return NetworkStringSerializer.Serialize((string)value);
            else if (type.IsEnum)
                return InvokeSerialize(typeof(NetworkEnumSerializer), type, new[] { value, typeof(byte) });
            else if (typeof(IList).IsAssignableFrom(type))
                return InvokeSerialize(typeof(NetworkListSerializer), type, new[] { value, encodeSubType });
            else
                return InvokeSerialize(typeof(NetworkClassSerializer), type, new[] { value });
        }

        /// <summary>
        /// Deserialize an object depending on its type
        /// </summary>
        /// <param name="type">The type of the object to deserialize</param>
        /// <param name="array">The byte array representing the object</param>
        /// <param name="shift">The shift to apply to the byte array</param>
        /// <param name="decodeSubType">Optional parameter; Used for ListSerializer</param>
        /// <returns>The object deserialized</returns>
        protected static object TypeDeserialize(Type type, byte[] array, ref int shift, bool decodeSubType = false)
        {
            object value;
//            var args = new object[] { array, shift };
            if (type.IsPrimitive)
            {
//                value = InvokeDeserialize(typeof(NetworkPrimitiveSerializer), type, args);
//                shift = (int)args[1];
                value = NetworkPrimitiveSerializer.Deserialize(array, ref shift, type);
            }
            else if (type == typeof(string))
                value = NetworkStringSerializer.Deserialize(array, ref shift);
            else if (type.IsEnum)
            {
//                args = new object[] { array, shift, typeof(byte) };
                value = NetworkEnumSerializer.Deserialize(array, ref shift, typeof(byte), type);
//                value = InvokeDeserialize(typeof(NetworkEnumSerializer), type, args);
//                shift = (int)args[1];
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
//                args = new object[] { array, shift, decodeSubType };
//                value = InvokeDeserialize(typeof(NetworkListSerializer), type, args);
//                shift = (int)args[1];
                value = NetworkListSerializer.Deserialize(array, ref shift, type, decodeSubType);
            }
            else
            {
//                value = InvokeDeserialize(typeof(NetworkClassSerializer), type, args);
//                shift = (int)args[1];
                value = NetworkClassSerializer.Deserialize(array, ref shift, type);
            }
            return value;
        }
    }
}
