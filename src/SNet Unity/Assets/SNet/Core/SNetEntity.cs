using SNet.Core.Models.Router;
using UnityEngine;

namespace SNet.Core
{
    [RequireComponent(typeof(SNetIdentity))]
    public abstract class SNetEntity : MonoBehaviour
    {
        public bool IsLocalClient { get; private set; } // To know if we are going to Predict or Interpolation the entity
        public bool IsServer => SNetManager.IsServerActive;
        public bool IsClient => SNetManager.IsClientActive;

        protected readonly SNetManager SNetManager = SNetManager.Instance;
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
            NetworkRouter.Register("0", callback); // TODO change to NetworkRouter.Register(??identity??, callback);  (V)_(;,,;)_(V)
        }

        #region SERVER STUFF

        protected void ServerBroadcastSerializable(byte[] data)
        {
            NetworkRouter.Send("0", data); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        //protected abstract void OnServerReceive(byte[] data);

        //protected abstract void ServerSend(object target, byte[] data);

        //protected abstract void ServerBroadcast(byte[] data);

        #endregion

        #region CLIENT STUFF
        
        protected void ClientSendSerializable(byte[] data)
        {
            NetworkRouter.Send("0", data); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        //protected abstract void OnClientReceive(byte[] data);

        //protected abstract void ClientSend(byte[] data);

        //protected abstract void InternalClientReceive(uint peerId, byte[] data);

        #endregion
    }
}