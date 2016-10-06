using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Basic
{
    /// <summary>
    /// Basic implementation of a class inheriting ISearchResult
    /// </summary>
    public class BasicSearchResult : ISearchResult
    {
        /// <summary>
        /// Memory address of result
        /// </summary>
        public ulong Address { get; set; }

        /// <summary>
        /// Value of result
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// Converts BasicSearchResult to byte array
        /// </summary>
        /// <param name="bytes">Uninitialized byte array</param>
        public void ToByteArray(out byte[] bytes)
        {
            if (Value == null || Value.Length == 0)
            {
                bytes = null;
                return;
            }

            bytes = new byte[sizeof(ulong) + Value.Length];

            // Copy address to first 8 bytes
            Array.Copy(Globals.DefaultBitConverter.GetBytes(Address), 0, bytes, 0, sizeof(ulong));
            // Copy value to last bytes
            Array.Copy(Value, 0, bytes, sizeof(ulong), Value.Length);
        }

        /// <summary>
        /// Loads Address and Value from byte array
        /// </summary>
        /// <param name="bytes">Byte array to load from</param>
        public void FromByteArray(byte[] bytes)
        {
            Value = new byte[bytes.Length - sizeof(ulong)];

            // Copy last bytes to Value
            Array.Copy(bytes, sizeof(ulong), Value, 0, Value.Length);

            // Convert first 8 bytes to ulong
            Address = Globals.DefaultBitConverter.ToUInt64(bytes, 0);
        }

    }
}
