using SNet.Core.Models.Router;
using UnityEngine;

namespace SNet.Core.Models
{
    [RequireComponent(typeof(SNetIdentity))]
    public abstract class SNetEntity : MonoBehaviour
    {
        public bool IsLocalClientPredictive { get; private set; } // To know if we are going to Predict or Interpolation the entity
        protected static bool IsServer => SNetManager.IsServer;
        protected static bool IsClient => SNetManager.IsClient;

        protected string InternalId => $"{Identity.Id}.{ComponentId}";

        protected SNetIdentity Identity;
        protected int ComponentId;

        public void Initialize(int componentId)
        {
            // Get identity from registration
            Identity = GetComponent<SNetIdentity>();
            
            IsLocalClientPredictive = Identity.isPredictive & Identity.IsLocalClient;

            ComponentId = componentId;

            Setup();
        }

        protected abstract void Setup();

        protected void NetworkRouterRegister(RouterCallback callback)
        {
            if (Identity == null)
            {
                Debug.LogError("Cannot register callback because Entity is not initialized yet.");
                return;
            }
            
            NetworkRouter.Register(InternalId, callback); // TODO change to NetworkRouter.Register(??identity??, callback);  (V)_(;,,;)_(V)
        }

        #region SERVER STUFF

        protected void ServerBroadcastSerializable(byte[] data)
        {
            if (Identity == null)
            {
                Debug.LogError("Cannot send data because Entity is not initialized yet.");
                return;
            }
            
            NetworkRouter.Send(InternalId, data); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        //protected abstract void OnServerReceive(byte[] data);

        //protected abstract void ServerSend(object target, byte[] data);

        //protected abstract void ServerBroadcast(byte[] data);

        #endregion

        #region CLIENT STUFF
        
        protected void ClientSendSerializable(byte[] data)
        {
            if (Identity == null)
            {
                Debug.LogError("Cannot send data because Entity is not initialized yet.");
                return;
            }
  
            NetworkRouter.Send(InternalId, data); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        //protected abstract void OnClientReceive(byte[] data);

        //protected abstract void ClientSend(byte[] data);

        //protected abstract void InternalClientReceive(uint peerId, byte[] data);

        #endregion
    }
}