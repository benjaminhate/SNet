namespace SNet.Core.Models.Network
{
    public class ClientEventData
    {
        public ClientEventData(byte channelId, uint peerId)
        {
            ChannelId = channelId;
            PeerId = peerId;
        }

        /// <summary>
        /// The content of the data received
        /// Only set when receiving event
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
    }
}