using SNet.Core.Common;

namespace SNet.Core.Models.Router
{
    public partial class ChannelType : Enumeration
    {
        public static ChannelType Base = new ChannelType(0, "Base");
        public static ChannelType SNetIdentity = new ChannelType(1, "SNetIdentity");
        
        public ChannelType(int id, string name) : base(id, name)
        {
        }

        public static implicit operator byte(ChannelType type)
        {
            return (byte) type.Value;
        }
    }
}