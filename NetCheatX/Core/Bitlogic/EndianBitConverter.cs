using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Bitlogic
{
    /// <summary>
    /// BitConverter wrapper with support for Big and Little Endian conversion
    /// </summary>
    public class EndianBitConverter
    {
        private Types.Endian _endian;

        /// <summary>
        /// Endianness of EndianBitConverter instance
        /// </summary>
        public Types.Endian Endianness { get { return _endian; } }

        /// <summary>
        /// Set the endianness to be used
        /// </summary>
        /// <param name="endian">An Endianness to be used</param>
        public EndianBitConverter(Types.Endian endian)
        {
            _endian = endian;
        }


        /// <summary>
        /// Flips byte array if the CPU endianness does not match the selected endianness
        /// </summary>
        /// <param name="value">A byte array</param>
        /// <param name="index">Starting index of the section</param>
        /// <param name="length">Number of elements in the section</param>
        private void _flipBytes(ref byte[] value, int index, int length)
        {
            if (_endian == Types.Endian.BigEndian && BitConverter.IsLittleEndian ||
                _endian == Types.Endian.LittleEndian && !BitConverter.IsLittleEndian)
                Array.Reverse(value, index, length);
        }

        #region Double Int64 Conversion

        /// <summary>
        /// Converts the specified double-precision floating point number to a 64-bit signed integer
        /// </summary>
        /// <param name="value">The number to convert</param>
        public long DoubleToInt64Bits(double value)
        {
            return BitConverter.DoubleToInt64Bits(value);
        }

        /// <summary>
        /// Converts the specified 64-bit signed number to a double-precision floating point number
        /// </summary>
        /// <param name="value">The number to convert</param>
        /// <returns></returns>
        public double Int64BitsToDouble(long value)
        {
            return BitConverter.Int64BitsToDouble(value);
        }

        #endregion

        #region GetBytes

        /// <summary>
        /// Converts a dynamic value (of the 10 supported by BitConverter.GetBytes()) to a byte array
        /// </summary>
        /// <param name="value">A byte array</param>
        /// <returns></returns>
        private byte[] _getBytes(dynamic value)
        {
            byte[] b = BitConverter.GetBytes(value);
            _flipBytes(ref b, 0, b.Length);
            return b;
        }

        /// <summary>
        /// Returns the specified Boolean value as an array of bytes
        /// </summary>
        /// <param name="value">A Boolean value</param>
        public byte[] GetBytes(bool value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified Unicode character value as an array of bytes
        /// </summary>
        /// <param name="value">A character to convert</param>
        public byte[] GetBytes(char value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(double value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(float value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(int value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(long value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(short value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(uint value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(ulong value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes
        /// </summary>
        /// <param name="value">The number to convert</param>
        public byte[] GetBytes(ushort value) { return _getBytes(value); }

        /// <summary>
        /// Returns the specified hexadecimal string representation of a number as an array of bytes
        /// </summary>
        /// <param name="value">The string to convert</param>
        public byte[] GetBytes(string value)
        {
            // Truncate any extra characters off length
            int length = value.Length - (value.Length % 2);

            // Initialize buffer
            byte[] buffer = new byte[length / 2];

            // Loop through every two characters and convert into byte
            for (int index = 0; index < length; index += 2)
                buffer[index / 2] = Convert.ToByte(value[index].ToString() + value[index + 1].ToString());

            // Flip if endianness dictates
            _flipBytes(ref buffer, 0, buffer.Length);

            return buffer;
        }

        #endregion

        #region Convert To

        /// <summary>
        /// Returns a Boolean value converted from one byte at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        public bool ToBoolean(byte[] value, int startIndex)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }

        /// <summary>
        /// Returns a Unicode character converted from two bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public char ToChar(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 2);
            return BitConverter.ToChar(value, startIndex);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public double ToDouble(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 8);
            return BitConverter.ToDouble(value, startIndex);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public float ToSingle(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 4);
            return BitConverter.ToSingle(value, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public int ToInt32(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 4);
            return BitConverter.ToInt32(value, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public long ToInt64(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 8);
            return BitConverter.ToInt64(value, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public short ToInt16(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 2);
            return BitConverter.ToInt16(value, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public uint ToUInt32(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 4);
            return BitConverter.ToUInt32(value, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public ulong ToUInt64(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 8);
            return BitConverter.ToUInt64(value, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array
        /// </summary>
        /// <param name="value">An array</param>
        /// <param name="startIndex">The starting position within value</param>
        public ushort ToUInt16(byte[] value, int startIndex)
        {
            _flipBytes(ref value, startIndex, 2);
            return BitConverter.ToUInt16(value, startIndex);
        }

        /// <summary>
        /// Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation
        /// </summary>
        /// <param name="value">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        /// <param name="length">The number of array elements in value to convert</param>
        public string ToString(byte[] value, int startIndex, int length)
        {
            _flipBytes(ref value, startIndex, length);
            return BitConverter.ToString(value, startIndex, length);
        }

        /// <summary>
        /// Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation
        /// </summary>
        /// <param name="value">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        public string ToString(byte[] value, int startIndex)
        {
            return ToString(value, startIndex, value.Length - startIndex);
        }

        /// <summary>
        /// Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation
        /// </summary>
        /// <param name="value">An array of bytes</param>
        public string ToString(byte[] value)
        {
            return ToString(value, 0, value.Length);
        }

        #endregion

    }
}
