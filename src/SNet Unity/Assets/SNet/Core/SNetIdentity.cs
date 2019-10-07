using System;
using SNet.Core.Models.Router;
using UnityEngine;

namespace SNet.Core
{
    // Network Identity for SNet entities
    public class SNetIdentity : MonoBehaviour
    {
        public bool isPredictive;
        public string Id { get; private set; }

        public bool IsClient => _sNetManager.IsClientActive;
        public bool IsServer => _sNetManager.IsServerActive;

        private readonly SNetManager _sNetManager = SNetManager.Instance;

        private void Awake()
        {
            if (IsServer)
                Initialize(GenerateNewId());
            else
                Initialize("0");
        }

        public void Initialize(string id)
        {
            // TODO Initialize Called when Spawn Message is received on the client side
            Register(id);
            if (IsServer)
            {
                // TODO Send Spawn Message to Clients
                NetworkRouter.SendByChannel(ChannelType.SNetIdentity, id, null);
            }
        }

        private void Register(string id)
        {
            if (IsServer || (IsClient && !isPredictive))
            {
                Id = id;
                InitializeEntities();
            }
            else
            {
                // TODO Send message to server and InitializeEntities on response
            }
        }

        private void InitializeEntities()
        {
            var entities = GetComponents<SNetEntity>();
            foreach (var entity in entities)
            {
                entity.Initialize();
            }
        }

        private string GenerateNewId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}