using System;
using SNet.Core.Models.Router;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    public class SNetEvent<TObj, TSerializableObj> : SNetEntity where TSerializableObj : ISNetSerialization, ISNetConvertible<TObj>, new()
    {
        public UnityEvent<T> clientRecieveCallback;
        public UnityEvent<T> serverRecieveCallback;
        
        protected new void Awake()
        {
            if (IsClient && clientRecieveCallback != null)
            {
                // TODO Change to NetworkRouter.RegisterClientCallback(identity.Id, clientEventCallbacks);
                NetworkRouter.Register(ChannelType.Base, HeaderType.Base, ((id, value) => clientRecieveCallback?.Invoke((T)value)), typeof(T));
            }

            if (IsServer && serverRecieveCallback != null)
            {
                // TODO Change to NetworkRouter.RegisterServerCallback(identity.Id, serverEventCallbacks);
            }
        }

        public override void ServerBroadcast(object data)
        {
            var obj = (TObj) data;
            var serializable = (TSerializableObj) new TSerializableObj().ConvertFrom(obj);
            ServerBroadcastSerializable(serializable);
        }

        public override void OnServerReceive(byte[] data)
        {
            var serializable = (TSerializableObj) data;
            var obj = serializable.ConvertTo();
            clientEventCallbacks?.Invoke(obj);
        }

        public override void ServerSend(object target, object data)
        {
            throw new NotImplementedException();
        }

        public override void OnClientReceive(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void ClientSend(object data)
        {
            throw new NotImplementedException();
        }
    }
}