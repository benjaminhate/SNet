﻿using UnityEditor.Callbacks;
using UnityEngine;

namespace SNet.Core.Editor
{
    public static class NetworkScenePostProcess
    {
        [PostProcessScene]
        public static void OnPostProcessScene()
        {
            var identities = Object.FindObjectsOfType<SNetIdentity>();
            foreach (var identity in identities)
            {
                if (!VerifyAssetId(identity.AssetId))
                {
                    Debug.LogWarning($"GameObject {identity.name} has invalid assetId");
                }
                
                identity.gameObject.SetActive(false);
            }
        }

        private static bool VerifyAssetId(string assetId)
        {
            return assetId != null;
        }
    }
}