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

        /// <summary>
        /// Content is set only when receiving data
        /// </summary>
        public byte[] Content { get; set; }
        /// <summary>
        /// The id of the channel where the data is received
        /// </summary>
        public byte ChannelId { get; }
        /// <summary>
        /// The id of the peer sending the data
        /// </summary>
        public uint PeerId { get; }
        /// <summary>
        /// The IP address of the peer sending the data
        /// </summary>
        public string PeerIp { get; }
    }
}