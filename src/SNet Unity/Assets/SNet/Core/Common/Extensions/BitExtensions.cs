using System;

namespace SNet.Core.Common.Extensions
{
    public static class BitExtensions
    {
        #region BitWise Helper
        /// <summary>
        /// Check if the index is valid for a byte operation (between 0 and 7)
        /// </summary>
        /// <param name="index">The index to check</param>
        /// <returns>A valid index</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the index is invalid</exception>
        private static int ValidIndex(int index)
        {
            if (index < 0 || index > 7)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be in the range of 0-7.");
            return index;
        }

        /// <summary>
        /// Create a Bitwise Mask from a byte index
        /// </summary>
        /// <param name="index">The index for the mask creation</param>
        /// <returns>The bitwise mask</returns>
        private static byte CreateMask(int index)
        {

            return (byte)(1 << index);
        }

        /// <summary>
        /// Create a valid Bitwise Mask
        /// </summary>
        /// <param name="index">The index for mask creation</param>
        /// <returns>A valid Bitwise Mask</returns>
        private static byte CreateValidMask(int index)
        {
            var i = ValidIndex(index);
            return CreateMask(i);
        }

        /// <summary>
        /// Check if the bit at index is true
        /// </summary>
        /// <param name="b">The byte to check on</param>
        /// <param name="index">The index to look for</param>
        /// <returns>True if the bit is set</returns>
        public static bool IsSet(this byte b, int index)
        {
            var m = CreateValidMask(index);
            return (b & m) != 0;
        }

        /// <summary>
        /// Set the bit at index to the bool value
        /// </summary>
        /// <param name="b">The byte to modify</param>
        /// <param name="index">The index to use</param>
        /// <param name="value">The value to set</param>
        /// <returns>The new byte value</returns>
        public static byte SetTo(this byte b, int index, bool value)
        {
            var m = CreateValidMask(index);

            if (value)
            {
                b |= m;
            }
            else
            {
                b &= (byte)~m;
            }

            return b;
        }

        /// <summary>
        /// Set a byte with the values in params
        /// </summary>
        /// <param name="b">The byte to modify</param>
        /// <param name="values">An array of bool values to set</param>
        /// <returns>The new byte value</returns>
        /// <exception cref="Exception">When the values in params is too big for a byte (more than 8)</exception>
        public static byte SetToAll(this byte b, params bool[] values)
        {
            if (values.Length > 8)
                throw new Exception("Too many values in input.\nMust be of maximal length : 8");

            for(var i = 0; i < values.Length; i++)
            {
                b = b.SetTo(i, values[i]);
            }

            return b;
        }

        /// <summary>
        /// Toggle the bool value of a bit at index
        /// </summary>
        /// <param name="b">The byte to modify</param>
        /// <param name="index">The index to toggle</param>
        /// <returns>The new byte value</returns>
        public static byte Toggle(this byte b, int index)
        {
            var m = CreateValidMask(index);

            b ^= m;

            return b;
        }
        #endregion

        #region BitConverter Shifting
        /// <summary>
        /// Get the next ushort value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The ushort value</returns>
        public static ushort ToUShort(byte[] array, ref int shift)
        {
            var val = BitConverter.ToUInt16(array, shift);
            shift += sizeof(ushort);
            return val;
        }
        /// <summary>
        /// Get the next uint value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The uint value</returns>
        public static uint ToUInt(byte[] array, ref int shift)
        {
            var val = BitConverter.ToUInt32(array, shift);
            shift += sizeof(uint);
            return val;
        }
        /// <summary>
        /// Get the next ulong value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The ulong value</returns>
        public static ulong ToULong(byte[] array, ref int shift)
        {
            var val = BitConverter.ToUInt64(array, shift);
            shift += sizeof(ulong);
            return val;
        }

        /// <summary>
        /// Get the next short value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The short value</returns>
        public static short ToShort(byte[] array, ref int shift)
        {
            var val = BitConverter.ToInt16(array, shift);
            shift += sizeof(short);
            return val;
        }
        /// <summary>
        /// Get the next int value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The int value</returns>
        public static int ToInt(byte[] array, ref int shift)
        {
            var val = BitConverter.ToInt32(array, shift);
            shift += sizeof(int);
            return val;
        }
        /// <summary>
        /// Get the next long value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The long value</returns>
        public static long ToLong(byte[] array, ref int shift)
        {
            var val = BitConverter.ToInt64(array, shift);
            shift += sizeof(long);
            return val;
        }

        /// <summary>
        /// Get the next byte value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The byte value</returns>
        public static byte ToByte(byte[] array, ref int shift)
        {
            var val = array[shift];
            shift += sizeof(byte);
            return val;
        }
        /// <summary>
        /// Get the next bool value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The bool value</returns>
        public static bool ToBool(byte[] array, ref int shift)
        {
            var val = BitConverter.ToBoolean(array, shift);
            shift += sizeof(bool);
            return val;
        }
        /// <summary>
        /// Get the next char value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The char value</returns>
        public static char ToChar(byte[] array, ref int shift)
        {
            var val = BitConverter.ToChar(array, shift);
            shift += sizeof(char);
            return val;
        }

        /// <summary>
        /// Get the next float value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The float value</returns>
        public static float ToFloat(byte[] array, ref int shift)
        {
            var val = BitConverter.ToSingle(array, shift);
            shift += sizeof(float);
            return val;
        }
        /// <summary>
        /// Get the next double value from a byte array with a shift
        /// Modify the shift accordingly 
        /// </summary>
        /// <param name="array">The full byte array</param>
        /// <param name="shift">The shift to use on the array</param>
        /// <returns>The double value</returns>
        public static double ToDouble(byte[] array, ref int shift)
        {
            var val = BitConverter.ToDouble(array, shift);
            shift += sizeof(double);
            return val;
        }
        #endregion

        #region BitConverter Optimisation
        public static byte[] GetBytes(long val, out byte type)
        {
            if (val < int.MaxValue)
                return GetBytes((int)val, out type);
            type = 0;
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(int val, out byte type)
        {
            if (val < short.MaxValue)
                return GetBytes((short)val, out type);
            type = 1;
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(short val, out byte type)
        {
            if (val < byte.MaxValue)
                return GetBytes((sbyte)val, out type);
            type = 2;
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(sbyte val, out byte type)
        {
            return GetBytes((byte)val, out type);
        }

        public static byte[] GetBytes(ulong val, out byte type)
        {
            if (val < uint.MaxValue)
                return GetBytes((uint)val, out type);
            type = 0;
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(uint val, out byte type)
        {
            if (val < ushort.MaxValue)
                return GetBytes((ushort)val, out type);
            type = 1;
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(ushort val, out byte type)
        {
            if (val < byte.MaxValue)
                return GetBytes((byte)val, out type);
            type = 2;
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(byte val, out byte type)
        {
            type = 3;
            var a = new byte[1];
            a[0] = val;
            return a;
        }

        public static byte[] GetBytes(double val, out byte type)
        {
            if (val < float.MaxValue)
                return GetBytes((float)val, out type);
            type = 0;
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(float val, out byte type)
        {
            type = 1;
            return BitConverter.GetBytes(val);
        }
        #endregion
    }
}
