using System.Linq;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SNet.Core.Editor
{
    public class SNetScenePostProcess : MonoBehaviour
    {
        [PostProcessScene]
        public static void OnPostProcessScene()
        {
            var identities = FindObjectsOfType<SNetIdentity>();
            var sceneId = 1u;
            
            foreach (var identity in identities.OrderBy(id => id.name))
            {
                if (identity.GetComponent<SNetManager>() != null)
                {
                    Debug.LogError($"SNetManager has a component SNetIdentity. This will cause the SNetManager to be disabled, so it is not recommended.");
                }
                
//                Debug.Log($"Setting scene ID {sceneId} for {identity.name}");
                identity.gameObject.SetActive(false);
                identity.SetSceneId(sceneId++);
            }
        }
    }
}