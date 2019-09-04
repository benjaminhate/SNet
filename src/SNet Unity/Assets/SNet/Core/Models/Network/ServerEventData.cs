namespace SNet.Core.Models.Network
{
    public class ServerEventData
    {
        public ServerEventData(byte channelId, uint peerId, string peerIp)
        {
            ChannelId = channelId;
            PeerId = peerId;
            PeerIp = peerIp;
        }

        public byte[] Content { get; set; }
        public byte ChannelId { get; }
        public uint PeerId { get; }
        public string PeerIp { get; }
    }
}