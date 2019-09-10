namespace SNet.Core
{
    public interface ISNetConvertible<T>
    {
        T ConvertTo();
        object ConvertFrom(T obj);
    }
}