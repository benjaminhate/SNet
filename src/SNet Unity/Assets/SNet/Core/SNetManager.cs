using System;
using SNet.Core.Common;
using SNet.Core.Models.Network;
using SNet.Core.Models.Router;
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

        public ClientNetwork Client { get; private set; }
        public ServerNetwork Server { get; private set; }

        public bool IsServerActive => _isServer &&  Server != null && Server.IsActive;
        public bool IsClientActive => _isClient && Client != null && Client.IsActive;

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
                Client.Update();
            if(IsServerActive)
                Server.Update();
        }

        private void OnDestroy()
        {
            if(IsClientActive)
                Client.Quit();
            if(IsServerActive)
                Server.Quit();
        }

        #region Client
        public static void StartClient()
        {
            Instance.StartClientNetwork();
        }
        
        private void StartClientNetwork()
        {
            _isClient = true;
            Client = new ClientNetwork();
            
            Client.OnConnect += data => Debug.Log($"Connected to server {data.PeerId}");
            Client.OnReceive += data => Debug.Log($"Received message from server {data.PeerId}");
            
            Client.Create();
            
            if(connectClientOnStart)
                Client.Connect(networkAddress, networkPort, maxChannels);
        }
        #endregion

        #region Server
        public static void StartServer()
        {
            Instance.StartServerNetwork();
        }

        private void StartServerNetwork()
        {
            _isServer = true;
            Server = new ServerNetwork();

            Server.OnConnect += ServerOnClientConnect;
            Server.OnDisconnect += ServerOnClientDisconnect;
            Server.OnTimeout += ServerOnClientTimeout;
            Server.OnReceive += ServerOnClientReceive;

            Server.Listen(networkAddress, networkPort, maxConnections, maxChannels);
        }

        internal virtual void ServerOnClientConnect(ServerEventData data)
        {
            Debug.Log($"Client {data.PeerId} connected with address {data.PeerIp}");
            Server.AddToFilter(data.PeerId);
        }

        internal virtual void ServerOnClientDisconnect(ServerEventData data)
        {
            
        }

        internal virtual void ServerOnClientTimeout(ServerEventData data)
        {
            
        }

        internal virtual void ServerOnClientReceive(ServerEventData data)
        {
            
        }
        #endregion

        public SNetIdentity RegisterIdentity(SNetEntity entity)
        {
            return new SNetIdentity{
                Id = $"{entity.name}.{entity.GetType().Name}"
            };
        }

        
    }
}