using SNet.Core.Common;

namespace SNet.Core.Models.Router
{
    public partial class HeaderType : Enumeration 
    {
        public static HeaderType Base = new HeaderType(0, "Base");
        
        public HeaderType(int id, string name) : base(id, name)
        {
        }
        
        public static implicit operator byte(HeaderType type)
        {
            return (byte) type.Value;
        }
    }
}
