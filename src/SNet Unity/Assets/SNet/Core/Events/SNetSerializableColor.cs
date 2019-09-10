using System;
using SNet.Core.Common.Serializer;
using UnityEngine;

namespace SNet.Core.Events
{
    [Serializable]
    public class SNetSerializableColor : ISNetSerialization, ISNetConvertible<Color>
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public static implicit operator Color(SNetSerializableColor color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }

        public static implicit operator SNetSerializableColor(Color color)
        {
            return new SNetSerializableColor(color);
        }

        public SNetSerializableColor(){ }

        public SNetSerializableColor(Color color)
        {
            a = color.a;
            b = color.b;
            g = color.g;
            r = color.r;
        }

        public byte[] Serialize()
        {
            return NetworkBinary.Serialize(this);
        }

        object ISNetSerialization.Deserialize(byte[] array)
        {
            return Deserialize(array);
        }

        public SNetSerializableColor Deserialize(byte[] array)
        {
            var shift = 0;
            return NetworkBinary.Deserialize<SNetSerializableColor>(array, ref shift);
        }

        public Color ConvertTo()
        {
            return (Color) this;
        }

        public object ConvertFrom(Color obj)
        {
            return (SNetSerializableColor) obj;
        }
    }
}