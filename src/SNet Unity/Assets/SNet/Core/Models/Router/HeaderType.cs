namespace SNet.Core.Models.Router
{
    // Limited to 256 [0...255]
    // Is converted as byte
    public enum HeaderType
    {
        None,
        Ack,
        Inputs,
        WorldSnapshot,
        ModeUpdate,
        ModeTimer,
        GameLoaded,
        ServerStartup
    }
}
