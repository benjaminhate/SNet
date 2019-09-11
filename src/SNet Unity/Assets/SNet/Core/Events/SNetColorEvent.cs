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

        public override void InternalClientReceive(uint peerId, byte[] data)
        {
            var color = SNetColorSerializer.Deserialize(data);
            clientReceiveCallback?.Invoke(color);
        }
    }
}