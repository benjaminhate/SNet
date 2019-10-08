using System;
using SNet.Core.Models;
using SNet.Core.Models.Router;
using UnityEditor;
using UnityEngine;

namespace SNet.Core
{
    // Network Identity for SNet entities
    public class SNetIdentity : MonoBehaviour
    {
        public bool isPredictive;
        public string Id { get; private set; }

        private string _assetId;

        public string AssetId
        {
            get
            {
                #if UNITY_EDITOR
                if (_assetId == null)
                {
                    SetupID();
                }
                #endif
                return _assetId;
            }
        }

        public bool IsClient => _sNetManager.IsClientActive;
        public bool IsServer => _sNetManager.IsServerActive;

        private readonly SNetManager _sNetManager = SNetManager.Instance;

        private void Awake()
        {
            if (IsServer)
                Initialize(GenerateNewId());
        }

        public void Initialize(string id)
        {
            // TODO Initialize Called when Spawn Message is received on the client side
            Register(id);
            if (IsServer)
            {
                var trans = transform;
                // TODO Send Spawn Message to Clients
                var msg = new ObjectSpawnMessage
                {
                    Id = id,
                    AssetId = AssetId,
                    Position = trans.position,
                    Rotation = trans.rotation
                };
                NetworkRouter.SendByChannel(ChannelType.SNetIdentity, SNetManager.SpawnMessageHeader, msg);
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
        
        #if UNITY_EDITOR
        private void SetupID()
        {
            var path = AssetDatabase.GetAssetPath(gameObject);
            _assetId = AssetDatabase.AssetPathToGUID(path);
        }
        #endif
    }
}