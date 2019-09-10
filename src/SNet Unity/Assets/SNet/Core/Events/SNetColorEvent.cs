using System;
using UnityEngine;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class ColorEvent : UnityEvent<Color>{ }
    
    public class SNetColorEvent : SNetEvent<Color, SNetSerializableColor>
    {
        public new ColorEvent clientRecieveCallback;
        
        public new void ServerBroadcast(object color)
        {
            var arr = SNetColorSerializer.Serialize((Color)color);
            ServerBroadcastSerializable(arr);
        }

        public new void OnClientReceive(byte[] arr)
        {
            var color = SNetColorSerializer.Deserialize(arr);
            clientRecieveCallback?.Invoke(color);
        }
    }
}