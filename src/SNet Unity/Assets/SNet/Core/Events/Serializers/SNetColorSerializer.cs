using System.Collections.Generic;
using SNet.Core.Common.Serializer;
using UnityEngine;

namespace SNet.Core.Events.Serializers
{
    public static class SNetColorSerializer
    {
        public static byte[] Serialize(Color color)
        {
            var arr = new List<byte>();
            arr.AddRange(NetworkBinary.Serialize(color.r));
            arr.AddRange(NetworkBinary.Serialize(color.g));
            arr.AddRange(NetworkBinary.Serialize(color.b));
            arr.AddRange(NetworkBinary.Serialize(color.a));

            return arr.ToArray();
        }

        public static Color Deserialize(byte[] array)
        {
            var shift = 0;
            var r = NetworkBinary.Deserialize<float>(array, ref shift);
            var g = NetworkBinary.Deserialize<float>(array, ref shift);
            var b = NetworkBinary.Deserialize<float>(array, ref shift);
            var a = NetworkBinary.Deserialize<float>(array, ref shift);
            return new Color(r, g, b, a);
        }
    }
}