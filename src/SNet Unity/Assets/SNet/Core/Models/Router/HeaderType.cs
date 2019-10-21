using SNet.Core.Common;

namespace SNet.Core.Models.Router
{
    public partial class HeaderType : Enumeration 
    {
        public static HeaderType Base = new HeaderType(0, "Base");
        public static HeaderType Empty = new HeaderType(1, "");
        public static HeaderType SpawnMessage = Empty;
        public static HeaderType ReadyMessage = Empty;
        
        public HeaderType(int id, string name) : base(id, name)
        {
        }
        
        public static implicit operator string(HeaderType type)
        {
            return type.Name;
        }
    }
}
