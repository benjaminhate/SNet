using System;
using SNet.Core.Models.Router;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    public class SNetEvent<T> : SNetEntity
    {
        public UnityEvent<T> clientRecieveCallback;
        public UnityEvent<T> serverRecieveCallback;
        
        protected new void Start()
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
            base.Start();
        }

        public override void ServerBroadcast(object data)
        {
            throw new NotImplementedException();
        }

        public override void OnServerReceive(byte[] data)
        {
            throw new NotImplementedException();
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