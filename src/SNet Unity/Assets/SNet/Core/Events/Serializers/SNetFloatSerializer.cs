using System;

namespace SNet.Core.Events.Serializers
{
    public static class SNetFloatSerializer
    {

        public static byte[] Serialize(float data)
        {
            var arr = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(arr);

            return arr;
        }

        public static float Deserialize(byte[] array)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array);

            return BitConverter.ToSingle(array, 0);
        }
    }
}