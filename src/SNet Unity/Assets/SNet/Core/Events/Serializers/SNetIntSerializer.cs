using System;

namespace SNet.Core.Events
{
    public static class SNetIntSerializer
    {

        public static byte[] Serialize(int data)
        {
            var arr = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(arr);

            return arr;
        }

        public static int Deserialize(byte[] array)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array);

            return BitConverter.ToInt32(array, 0);
        }
    }
}