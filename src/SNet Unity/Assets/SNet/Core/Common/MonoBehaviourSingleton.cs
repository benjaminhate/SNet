using UnityEngine;

namespace SNet.Core.Common
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        private static bool _shuttingDown;
        private static T _instance;

        protected void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Initialized MonoBehaviourSingleton of type : " + typeof(T).Name);
                _instance = GetComponent<T>();
                _shuttingDown = false;
            }
        }

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!_shuttingDown) return _instance;
                
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                 "' already destroyed. Returning null.");
                return null;

            }
        }
 
 
        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }
 
 
        private void OnDestroy()
        {
            Debug.Log("Destroying MonoBehaviourSingleton of type : " + typeof(T).Name);
            _shuttingDown = true;
        }
    }
}