namespace SNet.Core.Models.Network
{
    public class ClientEventData
    {
        public ClientEventData(byte channelId, uint peerId)
        {
            ChannelId = channelId;
            PeerId = peerId;
        }

        public byte[] Content { get; set; }
        public byte ChannelId { get; }
        public uint PeerId { get; }
    }
}