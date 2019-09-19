using System;
using System.Collections.Generic;
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

        private Dictionary<byte, Dictionary<string, TypedCallbackHandlers>> _callbacks;

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
            _callbacks = new Dictionary<byte, Dictionary<string, TypedCallbackHandlers>>();
            _wrapper = new RouterWrapper();
        }

        private void RegisterCallback(string header, RouterCallback routerCallback)
        {
            RegisterCallbackByChannel(ChannelType.Base, header, routerCallback);
        }

        private void RegisterCallbackByChannel(byte channel, string header, RouterCallback routerCallback)
        {
            if (!_callbacks.ContainsKey(channel))
                _callbacks.Add(channel, new Dictionary<string, TypedCallbackHandlers>());

            if (!_callbacks[channel].ContainsKey(header))
                _callbacks[channel].Add(header, new TypedCallbackHandlers());

            _callbacks[channel][header].Callbacks.Add(new CallbackHandler
            {
                Id = GetId(channel, header, routerCallback),
                Func = routerCallback
            });
        }

        private void SendDataToPeerIdByChannel(byte channel, string header, byte[] obj, uint peerId, PacketFlags flags = PacketFlags.None)
        {
            var data = CreatePayload(header, obj);
            SendToNetwork?.Invoke(peerId, data, channel, flags);
        }

        private void SendDataToPeerId(string header, byte[] obj, uint peerId, PacketFlags flags = PacketFlags.None)
        {
            SendDataToPeerIdByChannel(ChannelType.Base, header, obj, peerId, flags);
        }

        private void SendDataByChannel(byte channel, string header, byte[] obj, PacketFlags flags = PacketFlags.None,
            bool filter = true)
        {
            var data = CreatePayload(header, obj);
            BroadcastToNetwork?.Invoke(data, channel, flags, filter);
        }

        private void SendData(string header, byte[] obj, PacketFlags flags = PacketFlags.None, bool filter = true)
        {
            SendDataByChannel(ChannelType.Base, header, obj, flags, filter);
        }

        private byte[] CreatePayload(string header, byte[] data)
        {
            _wrapper.CreateRaw(data, header);
            return _wrapper.Raw;
        }

        private void Publish(uint peerId, byte channel, string header, byte[] array)
        {
            var structure = GetCallbacks(channel, header);

            structure?.Callbacks.ForEach(c => c.Func?.Invoke(peerId, array));
        }

        private void UnRegisterCallback(string header, RouterCallback routerCallback)
        {
            UnRegisterCallbackByChannel(ChannelType.Base, header, routerCallback);
        }

        private void UnRegisterCallbackByChannel(byte channel, string header, RouterCallback routerCallback)
        {
            var structure = GetCallbacks(channel, header);
            if (structure == null) return;

            var id = GetId(channel, header, routerCallback);
            structure.Callbacks.RemoveAll(c => c.Id == id);
            if (structure.Callbacks.Count == 0) _callbacks[channel].Remove(header);
        }

        private TypedCallbackHandlers GetCallbacks(byte channel, string header)
        {
            return CheckCallbacks(channel, header) ? _callbacks[channel][header] : null;
        }

        private bool CheckCallbacks(byte channel, string header)
        {
            return _callbacks.ContainsKey(channel) && _callbacks[channel].ContainsKey(header);
        }

        private void PeerReceiveData(uint peerId, byte channelId, byte[] data)
        {
            _wrapper.CreatePayload(data);
            Debug.Log($"Message from channel {channelId} and header {_wrapper.Header}");
            Publish(peerId, channelId, _wrapper.Header, _wrapper.Payload);
        }


        public static void RegisterByChannel(byte channel, string header, RouterCallback routerCallback)
        {
            Instance.RegisterCallbackByChannel(channel, header, routerCallback);
        }

        public static void Register(string header, RouterCallback routerCallback)
        {
            Instance.RegisterCallback(header, routerCallback);
        }

        public static void SendByChannel(byte channel, string header, byte[] obj, uint peerId, PacketFlags flags = PacketFlags.None)
        {
            Instance.SendDataToPeerIdByChannel(channel, header, obj, peerId, flags);
        }

        public static void SendByChannel(byte channel, string header, byte[] obj, PacketFlags flags = PacketFlags.None,
            bool filter = true)
        {
            Instance.SendDataByChannel(channel, header, obj, flags, filter);
        }

        public static void Send(string header, byte[] obj, uint peerId, PacketFlags flags = PacketFlags.None)
        {
            Instance.SendDataToPeerId(header, obj, peerId, flags);
        }

        public static void Send(string header, byte[] obj, PacketFlags flags = PacketFlags.None, bool filter = true)
        {
            Instance.SendData(header, obj, flags, filter);
        }

        public static void UnRegisterByChannel(byte channel, string header, RouterCallback routerCallback)
        {
            Instance?.UnRegisterCallbackByChannel(channel, header, routerCallback);
        }

        public static void UnRegister(string header, RouterCallback routerCallback)
        {
            Instance.UnRegisterCallback(header, routerCallback);
        }

        private static string GetId(byte channel, string header, RouterCallback routerCallback)
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