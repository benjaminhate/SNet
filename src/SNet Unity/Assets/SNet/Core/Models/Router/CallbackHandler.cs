namespace SNet.Core.Models.Router
{
    public delegate void RouterCallback(uint peerId, byte[] data);
    
    public class CallbackHandler
    {
        public string Id;
        public RouterCallback Func;
    }
}