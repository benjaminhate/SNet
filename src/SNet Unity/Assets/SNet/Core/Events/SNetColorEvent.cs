using System;
using UnityEngine;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class ColorEvent : UnityEvent<Color>{ }
    
    public class SNetColorEvent : SNetEvent<Color>
    {
        public new ColorEvent clientReceiveCallback;

        public void Start()
        {
            //// Register du OnClientReceive en fonction du contexte ?
            //if(isCLient && clientReceiveCallback != null)
            //{
            //    base.OnClientReceive += HandleClientReceive;
            //}
        }

        public void ServerBroadcast(Color color)
        {
            var arr = SNetColorSerializer.Serialize(color);
            ServerBroadcastSerializable(arr);
        }

        private void HandleClientReceive(byte[] arr)
        {
            var color = SNetColorSerializer.Deserialize(arr);
            clientReceiveCallback?.Invoke(color);
        }
    }
}