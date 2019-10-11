using System;
using UnityEngine;

namespace SNet.Core.Models
{
    [Serializable]
    public struct SNetSceneId
    {

        [SerializeField] private uint value;

        public SNetSceneId(uint value)
        {
            this.value = value;
        }

        public bool IsEmpty()
        {
            return value == 0;
        }

        public bool IsValid()
        {
            return !IsEmpty();
        }

        public bool Equals(SNetSceneId other)
        {
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            return obj is SNetSceneId other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return (int)value;
        }

        public static bool operator==(SNetSceneId c1, SNetSceneId c2)
        {
            return c1.value == c2.value;
        }

        public static bool operator!=(SNetSceneId c1, SNetSceneId c2)
        {
            return c1.value != c2.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}