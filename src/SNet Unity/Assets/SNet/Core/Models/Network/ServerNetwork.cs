using System;
using System.Collections.Generic;
using System.Linq;
using SNet.Core.Plugins.ENet.Scripts;

namespace SNet.Core.Models.Network
{
    public class ServerNetwork
    {
        private readonly Host _host = new Host();
        private readonly List<Peer> _peers = new List<Peer>();
        private readonly List<Peer> _peerFilter = new List<Peer>();

        public delegate void NetworkEvent(ServerEventData data);

        public event NetworkEvent OnConnect;
        public event NetworkEvent OnDisconnect;
        public event NetworkEvent OnTimeout;
        public event NetworkEvent OnReceive;

        public string Address { get; private set; }
        public ushort Port { get; private set; }

        public void Listen(string host, ushort port, int maxConnections, int maxChannels)
        {
            var address = new Address();

            Port = port;
            address.Port = Port;
            var success = address.SetHost(host);
            if(!success) throw new ApplicationException($"Host {host} is not valid");
            
            _host.Create(address, maxConnections, maxChannels);

            Address = host;
        }

        public void Update()
        {
            try
            {
                var polled = false;
                // poll loop to retrieve more than one data per frame
                while (!polled)
                {
                    if (_host.CheckEvents(out var netEvent) <= 0)
                    {
                        if (_host.Service(0, out netEvent) <= 0)
                            break;
                        polled = true;
                    }

                    var data = new ServerEventData(netEvent.ChannelID, netEvent.Peer.ID, netEvent.Peer.IP);

                    switch (netEvent.Type)
                    {
                        case EventType.Connect:
                            AddPeer(netEvent.Peer);
                            OnConnect?.Invoke(data);
                            break;

                        case EventType.Disconnect:
                            RemovePeer(netEvent.Peer);
                            OnDisconnect?.Invoke(data);
                            break;

                        case EventType.Timeout:
                            RemovePeer(netEvent.Peer);
                            OnTimeout?.Invoke(data);
                            break;

                        case EventType.Receive:
                            var buffer = new byte[netEvent.Packet.Length];
                            netEvent.Packet.CopyTo(buffer);
                            netEvent.Packet.Dispose();
                            data.Content = buffer;
                            OnReceive?.Invoke(data);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("ERROR Update : " + e);
            }
        }

        private void AddPeer(Peer peer)
        {
            _peers.Add(peer);
        }

        private void RemovePeer(Peer peer)
        {
            _peers.Remove(peer);
            _peerFilter.Remove(peer);
        }

        private Peer FindPeer(uint peerId)
        {
            return _peers.Find(p => p.ID == peerId);
        }

        public void Send(uint peerId, byte[] data, byte channelId, PacketFlags flags)
        {
            try
            {
                var packet = default(Packet);
                packet.Create(data, flags);
                var peer = FindPeer(peerId);
                if (peer.IsSet)
                {
                    peer.Send(channelId, ref packet);
                }
            }
            catch(Exception e)
            {
                UnityEngine.Debug.Log("ERROR Send : " + e);
            }
        }

        private void Broadcast(byte[] data, byte channelId, PacketFlags flags, Peer[] peers)
        {
            try
            {
                var packet = default(Packet);
                packet.Create(data, flags);
                _host.Broadcast(channelId, ref packet, peers);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("ERROR Broadcast : " + e);
            }
        }

        public void Broadcast(byte[] data, byte channelId, PacketFlags flags, bool filter = false)
        {
            var peers = GetAllPeer(filter).ToArray();
            Broadcast(data, channelId, flags, peers);
        }

        public void Quit()
        {
            foreach (var p in _peers)
            {
                p.DisconnectNow(0);
            }
            _host.Flush();
            _host.Dispose();
        }

        public IEnumerable<uint> GetAllPeerId(bool filter = false)
        {
            var peers = GetAllPeer(filter);
            return peers.Select(peer => peer.ID).ToList();
        }

        public List<Peer> GetAllPeer(bool filter = false)
        {
            return filter ? _peerFilter : _peers;
        }

        public void AddToFilter(uint clientId)
        {
            var peer = FindPeer(clientId);
            _peerFilter.Add(peer);
        }

        public void RemoveFromFilter(uint clientId)
        {
            var peer = FindPeer(clientId);
            _peerFilter.Remove(peer);
        }
    }
}
