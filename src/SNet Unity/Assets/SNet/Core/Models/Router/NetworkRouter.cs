using System;
using System.Collections.Generic;
using SNet.Core.Common.Serializer;
using SNet.Core.Plugins.ENet.Scripts;
using UnityEngine;

namespace SNet.Core.Models.Router
{
    public class NetworkRouter
    {
        #region Singleton
        private static readonly Lazy<NetworkRouter> Lazy = new Lazy<NetworkRouter>(() => new NetworkRouter());
        public static NetworkRouter Instance => Lazy.Value;
        
        private NetworkRouter(){ }
        #endregion

        private RouterWrapper _wrapper;

        private Dictionary<byte, Dictionary<byte, TypedCallbackHandlers>> _callbacks;

        public delegate void SendEvent(uint peerId, byte[] data, byte channel, PacketFlags flags);
        public event SendEvent SendToNetwork;

        public delegate void BroadcastEvent(byte[] data, byte channel, PacketFlags flags, bool filter);
        public event BroadcastEvent BroadcastToNetwork;
        
        public delegate void PeerEvent(uint peerId);

        public static event PeerEvent OnPeerConnection;
        public static event PeerEvent OnPeerDisconnection;
        public static event PeerEvent OnPeerTimeout;

        public void Start()
        {
            Init();
        }

        public void RefreshState()
        {
            Init();
        }
        
        protected virtual void Init()
        {
            _callbacks = new Dictionary<byte, Dictionary<byte, TypedCallbackHandlers>>();
            _wrapper = new RouterWrapper();
        }

        private void RegisterCallback(byte channel, byte header, RouterCallback routerCallback, Type returnType)
        {
            if (!_callbacks.ContainsKey(channel))
                _callbacks.Add(channel, new Dictionary<byte, TypedCallbackHandlers>());

            if (!_callbacks[channel].ContainsKey(header))
                _callbacks[channel].Add(header, new TypedCallbackHandlers
                {
                    Type = returnType
                });

            _callbacks[channel][header].Callbacks.Add(new CallbackHandler
            {
                Id = GetId(channel, header, routerCallback),
                Func = routerCallback
            });
        }

        private void SendData<T>(byte channel, byte header, T obj, uint peerId, PacketFlags flags = PacketFlags.None)
        {
            var data = CreatePayload(header, obj);
            SendToNetwork?.Invoke(peerId, data, channel, flags);
        }

        private void SendData<T>(byte channel, byte header, T obj, PacketFlags flags = PacketFlags.None,
            bool filter = true)
        {
            var data = CreatePayload(header, obj);
            BroadcastToNetwork?.Invoke(data, channel, flags, filter);
        }

        private byte[] CreatePayload<T>(byte header, T obj)
        {
            var data = NetworkBinary.Serialize(obj);
            _wrapper.CreateRaw(data, header);
            return _wrapper.Raw;
        }

        private void Publish(uint peerId, byte channel, byte header, byte[] array)
        {
            var structure = GetCallbacks(channel, header);
            if (structure == null) return;

            var shift = 0;
            var type = structure.Type;
            var data = NetworkBinary.Deserialize(array, ref shift, type);
            structure.Callbacks.ForEach(c => c.Func?.Invoke(peerId, data));
        }

        private void UnRegisterCallback(byte channel, byte header, RouterCallback routerCallback)
        {
            var structure = GetCallbacks(channel, header);
            if (structure == null) return;

            var id = GetId(channel, header, routerCallback);
            structure.Callbacks.RemoveAll(c => c.Id == id);
            if (structure.Callbacks.Count == 0) _callbacks[channel].Remove(header);
        }

        private TypedCallbackHandlers GetCallbacks(byte channel, byte header)
        {
            return CheckCallbacks(channel, header) ? _callbacks[channel][header] : null;
        }

        private bool CheckCallbacks(byte channel, byte header)
        {
            return _callbacks.ContainsKey(channel) && _callbacks[channel].ContainsKey(header);
        }

        private void PeerReceiveData(uint peerId, byte channelId, byte[] data)
        {
            _wrapper.CreatePayload(data);
            Debug.Log($"Message from channel {channelId} and header {_wrapper.Header}");
            Publish(peerId, channelId, _wrapper.Header, _wrapper.Payload);
        }


        public static void Register(byte channel, byte header, RouterCallback routerCallback, Type returnType)
        {
            Instance.RegisterCallback(channel, header, routerCallback, returnType);
        }

        public static void Send<T>(byte channel, byte header, T obj, uint peerId, PacketFlags flags = PacketFlags.None)
        {
            Instance.SendData(channel, header, obj, peerId, flags);
        }

        public static void Send<T>(byte channel, byte header, T obj, PacketFlags flags = PacketFlags.None,
            bool filter = true)
        {
            Instance.SendData(channel, header, obj, flags, filter);
        }

        public static void UnRegister(byte channel, byte header, RouterCallback routerCallback)
        {
            Instance?.UnRegisterCallback(channel, header, routerCallback);
        }

        private static string GetId(byte channel, byte header, RouterCallback routerCallback)
        {
            if (routerCallback.Method.ReflectedType == null)
                throw new NullReferenceException("routerCallback.Method.ReflectedType cannot be null");

            return $"{channel}_{header}_{routerCallback.Method.ReflectedType.FullName}.{routerCallback.Method.Name}";
        }


        public static void PeerConnection(uint peerId)
        {
            OnPeerConnection?.Invoke(peerId);
        }

        public static void PeerDisconnection(uint peerId)
        {
            OnPeerDisconnection?.Invoke(peerId);
        }

        public static void PeerTimeout(uint peerId)
        {
            OnPeerTimeout?.Invoke(peerId);
        }

        public static void PeerReceive(uint peerId, byte channelId, byte[] data)
        {
            Instance.PeerReceiveData(peerId, channelId, data);
        }
    }
}