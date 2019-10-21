using System.Collections.Generic;
using System.Linq;
using SNet.Core.Common;
using SNet.Core.Models;
using SNet.Core.Models.Network;
using SNet.Core.Models.Router;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [SerializeField] private List<GameObject> spawnList;

        private bool _isClient;
        private bool _isServer;

        private List<SNetIdentity> _sceneObjects;

        public ClientNetwork Client { get; private set; }
        public ServerNetwork Server { get; private set; }

        public bool IsServerActive => _isServer && Server != null && Server.IsActive;
        public bool IsClientActive => _isClient && Client != null && Client.IsActive;

        public static bool IsClient => Instance.IsClientActive;
        public static bool IsServer => Instance.IsServerActive;
        
        public string NetworkAddress => networkAddress;
        public ushort NetworkPort => networkPort;
        
        private void Start()
        {
            if(dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            Application.runInBackground = runInBackground;

            foreach (var prefab in spawnList)
            {
                SNetScene.RegisterPrefab(prefab);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneObjectsProcess();
        }

        private void Update()
        {
            if(IsClientActive)
                Client.Update();
            if(IsServerActive)
                Server.Update();
            SNetScene.UpdateScene();
        }

        private void OnDestroy()
        {
            if(IsClientActive)
                Client.Quit();
            if(IsServerActive)
                Server.Quit();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneObjectsProcess();
        }

        private void SceneObjectsProcess()
        {
            LoadSceneObjects();

            if (IsServerActive)
                SpawnSceneObjects();
        }

        private void LoadSceneObjects()
        {
            _sceneObjects = Resources.FindObjectsOfTypeAll<SNetIdentity>().ToList();
        }

        private void SpawnSceneObjects()
        {
            foreach (var obj in _sceneObjects.Where(obj => obj.SceneId.IsValid()))
            {
                obj.gameObject.SetActive(true);
                // Spawn message will be send automatically by SNetIdentity on Awake
            }
        }

        private bool GetSceneObject(SNetSceneId sceneId, out GameObject sceneObject)
        {
            sceneObject = null;
            if (!sceneId.IsValid()) return false;

            var obj = _sceneObjects.Find(o => o.SceneId == sceneId);
            if (obj == null) return false;

            sceneObject = obj.gameObject;
            return true;
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
//            Client.OnReceive += data => Debug.Log($"Received message from server {data.PeerId}");
            
            NetworkRouter.RegisterByChannel(ChannelType.SNetIdentity, HeaderType.SpawnMessage, SpawnEntity);
            
            Client.Create();
            
            if(connectClientOnStart)
                Client.Connect(networkAddress, networkPort, maxChannels);
        }

        private void SpawnEntity(uint peerId, byte[] data)
        {
            var idMsg = new ObjectSpawnMessage();
            idMsg.Deserialize(data);
            GameObject newObj;
            if (GetSceneObject(idMsg.SceneId, out var prefab))
            {
                // Object is in Scene -> activate it
                newObj = prefab;
                newObj.transform.position = idMsg.Position;
                newObj.transform.rotation = idMsg.Rotation;
                newObj.SetActive(true);
            }
            else if(SNetScene.GetPrefab(idMsg.AssetId, out prefab))
            {
                // Object is Prefab -> Instantiate
                newObj = SNetScene.Spawn(prefab, idMsg.Position, idMsg.Rotation);
            }
            else
            {
                // No Object found with AssetId
                return;
            }
            newObj.GetComponent<SNetIdentity>().Initialize(idMsg.Id);
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
            
            NetworkRouter.RegisterByChannel(ChannelType.SNetReady, HeaderType.ReadyMessage, OnClientReadyMessageReceive);

            Server.Listen(networkAddress, networkPort, maxConnections, maxChannels);
        }

        private void OnClientReadyMessageReceive(uint peerId, byte[] data)
        {
            Debug.Log($"Client {peerId} is ready");
            foreach (var msg in from netId in FindObjectsOfType<SNetIdentity>() select new ObjectSpawnMessage
            {
                Id = netId.Id,
                AssetId = netId.AssetId,
                SceneId = netId.SceneId,
                Position = netId.transform.position,
                Rotation = netId.transform.rotation
            })
            {
                NetworkRouter.SendByChannel(ChannelType.SNetIdentity, HeaderType.SpawnMessage, msg, peerId);
            }
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

        public void FinishSceneLoad()
        {
            if (IsServer)
            {
                OnServerSceneLoaded(SNetScene.SceneName);
            }

            if (IsClient)
            {
                OnClientSceneLoaded();
            }
        }

        public virtual void OnServerSceneLoaded(string sceneName)
        {
            
        }

        public virtual void OnClientSceneLoaded()
        {
            var msg = new ReadyMessage();
            NetworkRouter.SendByChannel(ChannelType.SNetReady, HeaderType.ReadyMessage, msg);
            Debug.Log("Client ready !");
        }
    }
}