using System.Linq;

namespace SNet.Core.Models.Router
{
    public class RouterWrapper
    {
        public byte[] Raw { get; private set; }

        public byte Header { get; private set; }
        public byte[] Payload { get; private set; }

        public void CreatePayload(byte[] buffer)
        {
            Raw = buffer;
            Header = Raw[0];
            Payload = Raw.Skip(1).ToArray();
        }

        public void CreateRaw(byte[] array, byte header)
        {
            Payload = array;
            Header = header;

            var copyPayload = Payload != null;
            var rawLength = copyPayload ? Payload.Length + 1 : 1;

            Raw = new byte[rawLength];
            Raw[0] = Header;
            if (copyPayload)
            {
                Payload.CopyTo(Raw, 1);
            }
        }
    }
}
