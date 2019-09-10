using SNet.Core.Common.Serializer;
using UnityEngine;

namespace SNet.Core.Events
{
    public static class SNetColorSerializer
    {
        public static byte[] Serialize(Color color)
        {
            return NetworkBinary.Serialize(color);
        }

        public static Color Deserialize(byte[] array)
        {
            var shift = 0;
            return NetworkBinary.Deserialize<Color>(array, ref shift);
        }
    }
}