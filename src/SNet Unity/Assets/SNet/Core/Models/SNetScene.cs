using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SNet.Core.Models
{
    public class SNetScene
    {
        public static string SceneName;
        
        private static readonly Dictionary<SNetHash128, GameObject> GuidToPrefab = new Dictionary<SNetHash128, GameObject>();

        public static AsyncOperation LoadSceneOperation { get; private set; }
        
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

        public static void UpdateScene()
        {
            if (LoadSceneOperation == null)
                return;
            if (!LoadSceneOperation.isDone)
                return;

            SNetManager.Instance.FinishSceneLoad();
            LoadSceneOperation.allowSceneActivation = true;
            LoadSceneOperation = null;
        }

        public static void ChangeScene(string newSceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneName = newSceneName;
            LoadSceneOperation = SceneManager.LoadSceneAsync(newSceneName, loadSceneMode);
        }
        public static void ChangeScene(int newSceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneName = SceneManager.GetSceneAt(newSceneIndex).name;
            LoadSceneOperation = SceneManager.LoadSceneAsync(newSceneIndex, loadSceneMode);
        }
    }
}