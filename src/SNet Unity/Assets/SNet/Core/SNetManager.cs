using System;
using SNet.Core.Common;
using SNet.Core.Models.Network;
using UnityEngine;

namespace SNet.Core
{
    [AddComponentMenu("Network/SNet/SNet Manager")]
    public class SNetManager : MonoBehaviourSingleton<SNetManager>
    {
        [SerializeField] private string networkAddress = "localhost";
        [SerializeField] private ushort networkPort = 16490;
        [SerializeField] private int maxConnections = 4;
        [SerializeField] private int maxChannels = 4;

        [SerializeField] private bool connectClientOnStart = true;
        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private bool runInBackground = true;

        private bool _isClient;
        private bool _isServer;
        
        private ClientNetwork _client;
        private ServerNetwork _server;

        public bool IsServerActive => _isServer && _server != null;
        public bool IsClientActive => _isClient && _client != null;

        public string NetworkAddress => networkAddress;
        public ushort NetworkPort => networkPort;

        private void Start()
        {
            if(dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            Application.runInBackground = runInBackground;
        }

        private void Update()
        {
            if(IsClientActive)
                _client.Update();
            if(IsServerActive)
                _server.Update();
        }

        public static void StartClient()
        {
            Instance.StartClientNetwork();
        }

        public static void StartServer()
        {
            Instance.StartServerNetwork();
        }

        private void StartClientNetwork()
        {
            _isClient = true;
            _client = new ClientNetwork();
            
            _client.OnConnect += data => Debug.Log($"Connected to server {data.PeerId}");
            
            _client.Create();
            
            if(connectClientOnStart)
                _client.Connect(networkAddress, networkPort, maxChannels);
        }

        private void StartServerNetwork()
        {
            _isServer = true;
            _server = new ServerNetwork();

            _server.OnConnect += data => Debug.Log($"Client {data.PeerId} connected with address {data.PeerIp}");
            
            _server.Listen(networkAddress, networkPort, maxConnections, maxChannels);
        }
    }
}