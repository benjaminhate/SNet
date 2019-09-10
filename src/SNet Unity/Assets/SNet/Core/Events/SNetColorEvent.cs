using System;
using UnityEngine;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class ColorEvent : UnityEvent<Color>{ }
    
    public class SNetColorEvent : SNetEvent<Color>
    {
        public new ColorEvent clientReceiveCallback;
        
        public void ServerBroadcast(Color color)
        {
            var arr = SNetColorSerializer.Serialize(color);
            ServerBroadcastSerializable(arr);
        }

        private new void InternalClientReceive(byte[] arr)
        {
            var color = SNetColorSerializer.Deserialize(arr);
            clientReceiveCallback?.Invoke(color);
        }
    }
}