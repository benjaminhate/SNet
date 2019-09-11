using System;
using SNet.Core.Events.Serializers;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class IntEvent : UnityEvent<int> { }

    public class SNetIntEvent : SNetEvent
    {
        public IntEvent clientReceiveCallback;
        public IntEvent serverReceiveCallback;

        protected override void Setup()
        {
            clientReceive += OnClientReceive;
            serverReceive += OnServerReceive;
        }

        private void OnClientReceive(uint peerId, byte[] array)
        {
            clientReceiveCallback?.Invoke(SNetIntSerializer.Deserialize(array));
        }
        
        private void OnServerReceive(uint peerId, byte[] array)
        {
            serverReceiveCallback?.Invoke(SNetIntSerializer.Deserialize(array));
        }

        public void ServerBroadcast(int data)
        {
            var value = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);

            ServerBroadcastSerializable(value);
        }
    }
}