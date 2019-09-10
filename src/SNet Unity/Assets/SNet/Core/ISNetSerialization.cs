namespace SNet.Core
{
    public interface ISNetSerialization
    {
        byte[] Serialize();
        object Deserialize(byte[] array);
    }
}