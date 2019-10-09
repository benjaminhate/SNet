using SNet.Core.Models.Router;
using UnityEngine;

namespace SNet.Core
{
    [RequireComponent(typeof(SNetIdentity))]
    public abstract class SNetEntity : MonoBehaviour
    {
        public bool IsLocalClient { get; private set; } // To know if we are going to Predict or Interpolation the entity
        protected static bool IsServer => SNetManager.IsServer;
        protected static bool IsClient => SNetManager.IsClient;

        protected SNetIdentity Identity;

        public void Initialize()
        {
            // TODO Self Registration
            // IsLocalClient = true; // Get value from SNetManager
            // Get identity from registration
            Identity = GetComponent<SNetIdentity>();

            Setup();
        }

        public abstract void Setup();

        protected void NetworkRouterRegister(RouterCallback callback)
        {
            NetworkRouter.Register(Identity.Id, callback); // TODO change to NetworkRouter.Register(??identity??, callback);  (V)_(;,,;)_(V)
        }

        #region SERVER STUFF

        protected void ServerBroadcastSerializable(byte[] data)
        {
            NetworkRouter.Send(Identity.Id, data); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        //protected abstract void OnServerReceive(byte[] data);

        //protected abstract void ServerSend(object target, byte[] data);

        //protected abstract void ServerBroadcast(byte[] data);

        #endregion

        #region CLIENT STUFF
        
        protected void ClientSendSerializable(byte[] data)
        {
            NetworkRouter.Send(Identity.Id, data); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        //protected abstract void OnClientReceive(byte[] data);

        //protected abstract void ClientSend(byte[] data);

        //protected abstract void InternalClientReceive(uint peerId, byte[] data);

        #endregion
    }
}