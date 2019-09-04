namespace SNet.Core.Models.Router
{
    public delegate void RouterCallback(uint peerId, object value);
    
    public class CallbackHandler
    {
        public string Id;
        public RouterCallback Func;
    }
}