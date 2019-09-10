namespace SNet.Core.Interfaces
{
    public interface ISNetManager
    {
        void StartServer();
        void StartClient();

        void StopServer();
        void StopClient();

        ISNetMode? GetMode();
    }

    public interface IServerManager
    {
        void Listen(ushort port);
        void Stop();

        void Send(byte[] data, INetworkConnection conn, ISyncOption syncOption = ISyncOption.State);
        void Broadcast(byte[] data, ISyncOption syncOption = ISyncOption.State);
    }

    public interface IClientManager
    {
        void Connect(string address, ushort port);
        void Stop();

        void Send(byte[] data, ISyncOption syncOption = ISyncOption.State);
    }

    public interface IClientManagerReceiver
    {
        byte[] Receive();
    }

    public interface INetworkConnection
    {
        
    }

    public enum ISNetMode
    {
        Client,
        Server
    }

    public enum ISyncOption
    {
        State,
        Predict,
        Interpolate
    }

    public interface ISynchroManager
    {
        
    }

    public interface INetworkEntity
    {
        void Send<T>(T obj, ISyncOption syncOption, ISendOption sendOption);
        T Receive<T>(out ISyncOption syncOption, out ISendOption sendOption);
    }

    // For later
    public enum ISendOption
    {
        Direct,
        WorldSnapshot
    }
}