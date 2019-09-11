using SNet.Core.Models.Router;
using UnityEngine;

namespace SNet.Core
{
    public abstract class SNetEntity : MonoBehaviour
    {
        public SNetIdentity identity;
        public bool IsLocalClient { get; private set; } // To know if we are going to Predict or Interpolation the entity
        public bool IsServer => SNetManager.IsServerActive;
        public bool IsClient => SNetManager.IsClientActive;

        protected SNetManager SNetManager;
        
        protected void Awake()
        {
            SNetManager = SNetManager.Instance;
            // TODO Self Registration
            // IsLocalClient = true; // Get value from SNetManager
            // Get identity from registration
        }

        #region SERVER STUFF

        public abstract void OnServerReceive(byte[] data);

        public abstract void ServerSend(object target, byte[] data);

        public abstract void ServerBroadcast(byte[] data);

        public void ServerBroadcastSerializable(byte[] data)
        {
            NetworkRouter.Send(ChannelType.Base, HeaderType.Base, data); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        #endregion

        #region CLIENT STUFF

        public abstract void OnClientReceive(byte[] data);
        
        public abstract void ClientSend(byte[] data);

        protected abstract void InternalClientReceive(uint peerId, byte[] data);

        #endregion
    }
}