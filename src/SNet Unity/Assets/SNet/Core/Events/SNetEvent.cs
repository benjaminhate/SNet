using System;
using SNet.Core.Models.Router;
using UnityEngine;
using UnityEngine.Events;

namespace SNet.Core.Events
{
    public class SNetEvent<T> : SNetEntity
    {
        public UnityEvent<T> clientEventCallbacks;
        public UnityEvent<T> serverEventCallbacks;
        
        protected new void Start()
        {
            if (IsClient && clientEventCallbacks != null)
            {
                // TODO Change to NetworkRouter.RegisterClientCallback(identity.Id, clientEventCallbacks);
                NetworkRouter.Register(ChannelType.Base, HeaderType.Base, ((id, value) => clientEventCallbacks?.Invoke((T)value)), typeof(T));
            }

            if (IsServer && serverEventCallbacks != null)
            {
                // TODO Change to NetworkRouter.RegisterServerCallback(identity.Id, serverEventCallbacks);
            }
            base.Start();
        }

        public override void ServerBroadcast(object data)
        {
            throw new NotImplementedException();
        }

        public override void OnReceive(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}