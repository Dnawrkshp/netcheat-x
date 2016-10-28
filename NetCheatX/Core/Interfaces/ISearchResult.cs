using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// Search result plugin interface
    /// </summary>
    public interface ISearchResult
    {
        /// <summary>
        /// Memory address of result
        /// </summary>
        ulong Address { get; set; }

        /// <summary>
        /// Value of result
        /// </summary>
        byte[] Value { get; set; }

        /// <summary>
        /// Converts the ISearchResult into an array of bytes to be stored into a scan file
        /// </summary>
        /// <param name="bytes">Resulting byte array after conversion</param>
        void ToByteArray(out byte[] bytes);

        /// <summary>
        /// Loads byte array information into ISearchResult instance
        /// </summary>
        /// <param name="bytes">ISearchResult in a byte array</param>
        void FromByteArray(byte[] bytes);

    }
}
