using System;
using SNet.Core.Models.Router;
using UnityEngine;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class ColorEvent : UnityEvent<Color>{ }
    
    public class SNetColorEvent : SNetEvent<Color>
    {
        public new ColorEvent clientEventCallbacks;
        
        public new void ServerBroadcast(object data)
        {
            var color = (Color) data;
            var serializable = new SNetSerializableColor(color);
            ServerBroadcastSerializable(serializable);
        }

        public new void OnReceive(byte[] data)
        {
            var serializable = new SNetSerializableColor();
            var color = (Color) serializable.Deserialize(data);
            clientEventCallbacks?.Invoke(color);
        }
    }
}