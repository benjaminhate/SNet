using System;

namespace SNet.Core.Models
{
    // unrolled for your pleasure.
    [Serializable]
    public struct NetworkHash128
    {
        // struct cannot have embedded arrays..
        public byte i0;
        public byte i1;
        public byte i2;
        public byte i3;
        public byte i4;
        public byte i5;
        public byte i6;
        public byte i7;
        public byte i8;
        public byte i9;
        public byte i10;
        public byte i11;
        public byte i12;
        public byte i13;
        public byte i14;
        public byte i15;

        public void Reset()
        {
            i0 = 0;
            i1 = 0;
            i2 = 0;
            i3 = 0;
            i4 = 0;
            i5 = 0;
            i6 = 0;
            i7 = 0;
            i8 = 0;
            i9 = 0;
            i10 = 0;
            i11 = 0;
            i12 = 0;
            i13 = 0;
            i14 = 0;
            i15 = 0;
        }

        public bool IsValid()
        {
            return (i0 | i1 | i2 | i3 | i4 | i5 | i6 | i7 | i8 | i9 | i10 | i11 | i12 | i13 | i14 | i15) != 0;
        }

        private static int HexToNumber(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;
            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;
            return 0;
        }

        public static NetworkHash128 Parse(string text)
        {
            NetworkHash128 hash;

            // add leading zeros if required
            var l = text.Length;
            if (l < 32)
            {
                var tmp = "";
                for (var i = 0; i < 32 - l; i++)
                {
                    tmp += "0";
                }
                text = (tmp + text);
            }

            hash.i0 = (byte)(HexToNumber(text[0]) * 16 + HexToNumber(text[1]));
            hash.i1 = (byte)(HexToNumber(text[2]) * 16 + HexToNumber(text[3]));
            hash.i2 = (byte)(HexToNumber(text[4]) * 16 + HexToNumber(text[5]));
            hash.i3 = (byte)(HexToNumber(text[6]) * 16 + HexToNumber(text[7]));
            hash.i4 = (byte)(HexToNumber(text[8]) * 16 + HexToNumber(text[9]));
            hash.i5 = (byte)(HexToNumber(text[10]) * 16 + HexToNumber(text[11]));
            hash.i6 = (byte)(HexToNumber(text[12]) * 16 + HexToNumber(text[13]));
            hash.i7 = (byte)(HexToNumber(text[14]) * 16 + HexToNumber(text[15]));
            hash.i8 = (byte)(HexToNumber(text[16]) * 16 + HexToNumber(text[17]));
            hash.i9 = (byte)(HexToNumber(text[18]) * 16 + HexToNumber(text[19]));
            hash.i10 = (byte)(HexToNumber(text[20]) * 16 + HexToNumber(text[21]));
            hash.i11 = (byte)(HexToNumber(text[22]) * 16 + HexToNumber(text[23]));
            hash.i12 = (byte)(HexToNumber(text[24]) * 16 + HexToNumber(text[25]));
            hash.i13 = (byte)(HexToNumber(text[26]) * 16 + HexToNumber(text[27]));
            hash.i14 = (byte)(HexToNumber(text[28]) * 16 + HexToNumber(text[29]));
            hash.i15 = (byte)(HexToNumber(text[30]) * 16 + HexToNumber(text[31]));

            return hash;
        }

        public override string ToString()
        {
            return
                $"{i0:x2}{i1:x2}{i2:x2}{i3:x2}{i4:x2}{i5:x2}{i6:x2}{i7:x2}{i8:x2}{i9:x2}{i10:x2}{i11:x2}{i12:x2}{i13:x2}{i14:x2}{i15:x2}";
        }

        public static bool operator ==(NetworkHash128 a, NetworkHash128 b)
        {
            return a.i0 == b.i0 &&
                   a.i1 == b.i1 &&
                   a.i2 == b.i2 &&
                   a.i3 == b.i3 &&
                   a.i4 == b.i4 &&
                   a.i5 == b.i5 &&
                   a.i6 == b.i6 &&
                   a.i7 == b.i7 &&
                   a.i8 == b.i8 &&
                   a.i9 == b.i9 &&
                   a.i10 == b.i10 &&
                   a.i11 == b.i11 &&
                   a.i12 == b.i12 &&
                   a.i13 == b.i13 &&
                   a.i14 == b.i14 &&
                   a.i15 == b.i15;
        }

        public static bool operator !=(NetworkHash128 a, NetworkHash128 b)
        {
            return !(a == b);
        }
    }
}