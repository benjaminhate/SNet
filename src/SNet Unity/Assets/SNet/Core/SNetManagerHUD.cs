using UnityEngine;

namespace SNet.Core
{
    [AddComponentMenu("Network/SNet/SNet Manager HUD")]
    public class SNetManagerHUD : MonoBehaviour
    {
        [SerializeField] private bool showGUI = true;

        private SNetManager _manager;
        
        private void Awake()
        {
            _manager = SNetManager.Instance;
        }

        private void OnGUI()
        {
            if (!showGUI)
                return;
            var clientActive = _manager.IsClientActive;
            var serverActive = _manager.IsServerActive;
            var noConnection = !clientActive && !serverActive;
            
            if (noConnection)
            {
                if (GUILayout.Button("Client"))
                {
                    SNetManager.StartClient();
                }

                if (GUILayout.Button("Server"))
                {
                    SNetManager.StartServer();
                }   
            }
            else
            {
                if (clientActive)
                {
                    GUILayout.Label($"Client: address={_manager.NetworkAddress} port={_manager.NetworkPort}");
                }

                if (serverActive)
                {
                    GUILayout.Label($"Server: port={_manager.NetworkPort}");
                }
            }
        }
    }
}