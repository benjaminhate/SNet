using UnityEngine;

namespace SNet.Core
{
    // Network Identity for SNet entities
    public class SNetIdentity : MonoBehaviour
    {
        public bool serverOnly;
        
        public string Id { get; private set; }

        public bool IsClient => _sNetManager.IsClientActive;
        public bool IsServer => _sNetManager.IsServerActive;

        private readonly SNetManager _sNetManager = SNetManager.Instance;

        private void Awake()
        {
            Register();
        }

        private void Register()
        {
            // If not on server and set server only, the gameObject must be destroyed because it has no business here
            if(serverOnly && !IsServer)
                Destroy(gameObject);
            // TODO separate behaviours when IsClient, IsServer, IsSceneSpawned
        }
    }
}