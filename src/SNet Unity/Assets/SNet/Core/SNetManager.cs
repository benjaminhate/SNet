using System.Collections.Generic;
using System.Linq;
using SNet.Core.Common;
using SNet.Core.Models;
using SNet.Core.Models.Network;
using SNet.Core.Models.Router;
using UnityEditor;
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

        public static string SpawnMessageHeader => "";

        private void Start()
        {
            if(dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            Application.runInBackground = runInBackground;

            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneObjectsProcess();
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
            
            NetworkRouter.RegisterByChannel(ChannelType.SNetIdentity, SpawnMessageHeader, SpawnEntity);
            
            Client.Create();
            
            if(connectClientOnStart)
                Client.Connect(networkAddress, networkPort, maxChannels);
        }

        private void SpawnEntity(uint peerId, byte[] data)
        {
            var idMsg = new ObjectSpawnMessage();
            idMsg.Deserialize(data);
            GameObject newObj;
            if (IsSceneObject(idMsg, out var prefab))
            {
                // Object is in Scene -> activate it
                newObj = prefab;
                newObj.transform.position = idMsg.Position;
                newObj.transform.rotation = idMsg.Rotation;
                newObj.SetActive(true);
            }
            else if(prefab != null)
            {
                // Object is Prefab -> Instantiate
                newObj = NetworkScene.Spawn(prefab, idMsg.Position, idMsg.Rotation);
            }
            else
            {
                // No Object found with AssetId
                return;
            }
            newObj.GetComponent<SNetIdentity>().Initialize(idMsg.Id);
        }

        private bool IsSceneObject(ObjectSpawnMessage idMsg, out GameObject obj)
        {
            if (idMsg.SceneId.IsValid())
            {
                obj = _sceneObjects.Find(o => o.SceneId == idMsg.SceneId)?.gameObject;
                if (obj != null) return true;
            }

            if (idMsg.AssetId.IsValid())
            {
                obj = spawnList.Find(go => go.GetComponent<SNetIdentity>()?.AssetId == idMsg.AssetId);
                return false;
            }

            obj = null;
            return false;
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
    }
}