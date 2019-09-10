using SNet.Core.Plugins.ENet.Scripts;

namespace SNet.Core.Models.Network
{
    public class ClientNetwork
    {
        private Host _host;
        public Peer Peer { get; private set; }

        public delegate void NetworkEvent(ClientEventData data);

        public event NetworkEvent OnConnect;
        public event NetworkEvent OnDisconnect;
        public event NetworkEvent OnTimeout;
        public event NetworkEvent OnReceive;

        public ushort Port { get; private set; }
        public string Address { get; private set; }

        public bool IsActive => _host.IsSet;

        public ClientNetwork()
        {
            Init();
        }
        
        public void Init()
        {
            _host = new Host();
            Peer = new Peer();
        }
        
        public void Create()
        {
            _host.Create();
        }
        
        public void Connect(string hostName, ushort port, int maxChannels)
        {
            var address = new Address();

            Port = port;
            address.SetHost(hostName);
            address.Port = Port;

            Peer = _host.Connect(address, maxChannels);
            Address = Peer.IP;
        }

        public void Update()
        {
            if (!_host.IsSet) return;
            
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

                var data = new ClientEventData(netEvent.ChannelID, netEvent.Peer.ID);

                switch (netEvent.Type)
                {
                    case EventType.Connect:
                        OnConnect?.Invoke(data);
                        break;

                    case EventType.Disconnect:
                        OnDisconnect?.Invoke(data);
                        break;

                    case EventType.Timeout:
                        OnTimeout?.Invoke(data);
                        break;

                    case EventType.Receive:
                        // Get the byte[] from the netEvent.Packet
                        var buffer = new byte[netEvent.Packet.Length];
                        netEvent.Packet.CopyTo(buffer);
                        netEvent.Packet.Dispose();
                        data.Content = buffer;

                        OnReceive?.Invoke(data);
                        break;
                }
            }
        }

        public void Send(byte[] data, byte channelId, PacketFlags flags)
        {
            var packet = default(Packet);
            packet.Create(data, flags);
            if (Peer.IsSet)
            {
                Peer.Send(channelId, ref packet);
            }
        }

        public void Quit()
        {
            if(Peer.IsSet)
                Peer.DisconnectNow(0);
            _host.Flush();
            _host.Dispose();
        }
    }
}