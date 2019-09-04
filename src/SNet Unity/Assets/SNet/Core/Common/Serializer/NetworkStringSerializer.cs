using System.Collections.Generic;

namespace SNet.Core.Common.Serializer
{
    public class NetworkStringSerializer : NetworkSerializer
    {
        /// <summary>
        /// Serialize an object into a byte array
        /// </summary>
        /// <param name="str">The string to serialize</param>
        /// <returns>A byte array of the serialized string</returns>
        public static byte[] Serialize(string str)
        {
            var list = new List<byte>();

            if (str == null) str = "null";
            
            char[] cArr = str.ToCharArray();
            var length = cArr.Length;
            list.AddRange(GetBytes(length));
            for (var j = 0; j < length; j++)
            {
                list.AddRange(GetBytes(cArr[j]));
            }

            return list.ToArray();
        }

        /// <summary>
        /// Deserialize a byte array into a string
        /// </summary>
        /// <param name="array">The byte array</param>
        /// <param name="shift">The shift for the array</param>
        /// <returns>The string deserialized</returns>
        public static string Deserialize(byte[] array, ref int shift)
        {
            var length = (int)FromBytes(typeof(int), array, ref shift);
            var cArr = new char[length];
            for (var j = 0; j < length; j++)
            {
                cArr[j] = (char)FromBytes(typeof(char), array, ref shift);
            }

            var str = new string(cArr);
            return str == "null" ? null : str;
        }
    }
}
