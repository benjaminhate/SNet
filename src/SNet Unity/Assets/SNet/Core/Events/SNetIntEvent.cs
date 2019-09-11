//using System;
//using UnityEngine.Events;

//namespace SNet.Core.Events
//{
//    [Serializable] public class IntEvent : UnityEvent<int> { }

//    public class SNetIntEvent : SNetEvent<int>
//    {
//        public IntEvent clientReceiveCallback;
//        public IntEvent serverReceiveCallback;

//        private new void Awake()
//        {
//            clientReceive = clientReceiveCallback;
//            base.Awake();
//        }

//        public new void ClientSend(byte[] data)
//        {
//            throw new NotImplementedException();
//        }

//        public new void OnClientReceive(byte[] array)
//        {
//            clientReceiveCallback?.Invoke(SNetIntSerializer.Deserialize(array));
//        }

//        public void ServerBroadcast(int data)
//        {
//            var value = BitConverter.GetBytes(data);
//            if (BitConverter.IsLittleEndian)
//                Array.Reverse(value);

//            ServerBroadcastSerializable(value);
//        }
        
//        public new void ServerSend(object target, byte[] data)
//        {
//            throw new NotImplementedException();
//        }

//        public new void OnServerReceive(byte[] array)
//        {
//            serverReceiveCallback?.Invoke(SNetIntSerializer.Deserialize(array));
//        }
//    }
//}