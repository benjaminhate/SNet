using System.Reflection;
using SNet.Core.Models.Router;
using SNet.Core.Plugins.ENet.Scripts;
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
        
        protected void Start()
        {
            SNetManager = SNetManager.Instance;
            // TODO Self Registration
            IsLocalClient = true; // Get value from SNetManager
            // Get identity from registration
        }

        public abstract void ServerBroadcast(object data);

        public void ServerBroadcastSerializable(ISNetSerialization serialization)
        {
            NetworkRouter.Send(ChannelType.Base, HeaderType.Base, serialization.Serialize()); // TODO change to NetworkRouter.Send(identity.Id, data);
        }

        public abstract void OnReceive(byte[] data);
    }
}