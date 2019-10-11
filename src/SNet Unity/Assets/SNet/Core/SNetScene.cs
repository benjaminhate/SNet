using System.Collections.Generic;
using SNet.Core.Models;
using UnityEngine;

namespace SNet.Core
{
    public class SNetScene
    {
        private static readonly Dictionary<SNetHash128, GameObject> GuidToPrefab = new Dictionary<SNetHash128, GameObject>();
        
        public static GameObject Spawn(GameObject go, Vector3 position, Quaternion rotation)
        {
            return Object.Instantiate(go, position, rotation);
        }

        public static void RegisterPrefab(GameObject prefab)
        {
            var netId = prefab.GetComponent<SNetIdentity>();
            if (netId == null)
            {
                Debug.LogError($"Could not register prefab {prefab.name} because SNetIdentity component is missing.");
                return;
            }
            
            GuidToPrefab.Add(netId.AssetId, prefab);
        }

        public static bool GetPrefab(SNetHash128 assetId, out GameObject prefab)
        {
            prefab = null;
            if (!assetId.IsValid() || !GuidToPrefab.ContainsKey(assetId) || GuidToPrefab[assetId] == null)
                return false;

            prefab = GuidToPrefab[assetId];
            return true;
        }
    }
}