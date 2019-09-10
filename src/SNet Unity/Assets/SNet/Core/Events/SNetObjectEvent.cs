using System;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class ObjectEvent : UnityEvent<object>{ }
    
    public class SNetObjectEvent : SNetEvent<object, SNetSerializableObject>
    {
        public new ObjectEvent clientEventCallbacks;
    }
}