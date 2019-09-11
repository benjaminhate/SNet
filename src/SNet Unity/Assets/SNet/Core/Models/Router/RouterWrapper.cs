using System.Linq;
using SNet.Core.Common.Serializer;

namespace SNet.Core.Models.Router
{
    public class RouterWrapper
    {
        public byte[] Raw { get; private set; }

        public string Header { get; private set; }
        public byte[] Payload { get; private set; }

        public void CreatePayload(byte[] buffer)
        {
            Raw = buffer;
            var shift = 0;
            Header = NetworkBinary.Deserialize<string>(Raw, ref shift);
            Payload = Raw.Skip(shift).ToArray();
        }

        public void CreateRaw(byte[] array, string header)
        {
            Payload = array;
            Header = header;

            var headerByte = NetworkBinary.Serialize(Header);

            var copyPayload = Payload != null;
            var rawLength = copyPayload ? Payload.Length + headerByte.Length : headerByte.Length;

            Raw = new byte[rawLength];
            headerByte.CopyTo(Raw, 0);
            if (copyPayload)
            {
                Payload.CopyTo(Raw, headerByte.Length);
            }
        }
    }
}
