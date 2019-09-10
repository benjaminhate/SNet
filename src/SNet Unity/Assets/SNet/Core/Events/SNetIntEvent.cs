using System;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    [Serializable] public class IntEvent : UnityEvent<int> { }

    public class SNetIntEvent : SNetEvent<int>
    {
        public new IntEvent clientRecieveCallback;
        public new IntEvent serverRecieveCallback;
        
        public new void ClientSend(object data)
        {
            throw new NotImplementedException();
        }

        public new void OnClientReceive(byte[] array)
        {
            clientRecieveCallback?.Invoke(SNetIntSerializer.Deserialize(array));
        }

        public new void ServerBroadcast(object data)
        {
            var value = BitConverter.GetBytes((int)data);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);

            ServerBroadcastSerializable(value);
        }
        
        public new void ServerSend(object target, object data)
        {
            throw new NotImplementedException();
        }

        public new void OnServerReceive(byte[] array)
        {
            serverRecieveCallback?.Invoke(SNetIntSerializer.Deserialize(array));
        }
    }
}