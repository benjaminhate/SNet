using System;
using UnityEngine;

namespace SNet.Core.Models
{
    [Serializable]
    public struct NetworkSceneId
    {

        [SerializeField] private uint value;

        public NetworkSceneId(uint value)
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

        public bool Equals(NetworkSceneId other)
        {
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            return obj is NetworkSceneId other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return (int)value;
        }

        public static bool operator==(NetworkSceneId c1, NetworkSceneId c2)
        {
            return c1.value == c2.value;
        }

        public static bool operator!=(NetworkSceneId c1, NetworkSceneId c2)
        {
            return c1.value != c2.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}