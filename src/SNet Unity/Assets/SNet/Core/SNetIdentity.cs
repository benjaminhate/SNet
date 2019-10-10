using System;
using SNet.Core.Models;
using SNet.Core.Models.Router;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SNet.Core
{
    // Network Identity for SNet entities
    public class SNetIdentity : MonoBehaviour
    {
        public bool isPredictive;
        public string Id { get; private set; }

        [SerializeField] private NetworkHash128 assetId;

        [SerializeField] private NetworkSceneId sceneId;

        private int entityId;

        public NetworkSceneId SceneId => sceneId;

        public NetworkHash128 AssetId
        {
            get
            {
                #if UNITY_EDITOR
                if (!assetId.IsValid())
                {
                    SetupID();
                }
                #endif
                return assetId;
            }
        }

        private static bool IsClient => SNetManager.IsClient;
        private static bool IsServer => SNetManager.IsServer;
        
        private void Start()
        {
            if (IsServer)
                Initialize(GenerateNewId());
        }

        public void Initialize(string id)
        {
            // TODO Initialize Called when Spawn Message is received on the client side
            Register(id);
            if (!IsServer) return;
            
            var trans = transform;
            // TODO Send Spawn Message to Clients
            var msg = new ObjectSpawnMessage
            {
                Id = id,
                AssetId = AssetId,
                SceneId = SceneId,
                Position = trans.position,
                Rotation = trans.rotation
            };
            Debug.Log($"Sending spawn message : {msg.Id}, {msg.AssetId}, {msg.SceneId}");
            NetworkRouter.SendByChannel(ChannelType.SNetIdentity, SNetManager.SpawnMessageHeader, msg);
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
                entity.Initialize(entityId++);
            }
        }

        private string GenerateNewId()
        {
            return Guid.NewGuid().ToString();
        }

        public void SetSceneId(uint id)
        {
            sceneId = new NetworkSceneId(id);
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            SetupID();
        }

        private void SetupID()
        {
            if (ThisIsAPrefab())
            {
                SetSceneId(0);
                AssignAssetId(gameObject);
            }
            else if (ThisIsASceneObjectReferencingAPrefab(out var prefab))
            {
                AssignAssetId(prefab);
            }
            else
            {
                assetId.Reset();
            }
        }

        private bool ThisIsAPrefab()
        {
            return PrefabUtility.IsPartOfPrefabAsset(gameObject);
        }

        private bool ThisIsASceneObjectReferencingAPrefab(out GameObject prefab)
        {
            prefab = null;
            if (!PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject))
                return false;
            
            prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
            if (prefab != null) return true;
            
            Debug.LogError("Failed to find prefab parent for scene object [name:" + gameObject.name + "]");
            return false;
        }

        private void AssignAssetId(Object prefab)
        {
            var path = AssetDatabase.GetAssetPath(prefab);
            assetId = NetworkHash128.Parse(AssetDatabase.AssetPathToGUID(path));
        }
        #endif
    }
}