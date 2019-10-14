using System;
using SNet.Core.Events.Serializers;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class FloatEvent : UnityEvent<float> { }

    public class SNetFloatEvent : SNetEvent
    {
        public FloatEvent clientReceiveCallback;
        public FloatEvent serverReceiveCallback;

        protected override void PreSetup()
        {
            ClientReceive += OnClientReceive;
            ServerReceive += OnServerReceive;
        }

        private void OnClientReceive(uint peerId, byte[] array)
        {
            clientReceiveCallback?.Invoke(SNetFloatSerializer.Deserialize(array));
        }
        
        private void OnServerReceive(uint peerId, byte[] array)
        {
            serverReceiveCallback?.Invoke(SNetFloatSerializer.Deserialize(array));
        }

        public void ServerBroadcast(float data)
        {
            var value = SNetFloatSerializer.Serialize(data);
            ServerBroadcastSerializable(value);
        }

        public void ClientSend(float data)
        {
            var value = SNetFloatSerializer.Serialize(data);
            ClientSendSerializable(value);
        }
    }
}