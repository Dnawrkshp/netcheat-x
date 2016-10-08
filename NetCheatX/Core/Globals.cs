using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Static class containing variable constants
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Big Endian BitConverter
        /// </summary>
        public static Bitlogic.EndianBitConverter BigEndianBitConverter = new Bitlogic.EndianBitConverter(Types.Endian.BigEndian);

        /// <summary>
        /// Little Endian BitConverter
        /// </summary>
        public static Bitlogic.EndianBitConverter LittleEndianBitConverter = new Bitlogic.EndianBitConverter(Types.Endian.LittleEndian);

        /// <summary>
        /// Default BitConverter
        /// Used when saving binary information to files
        /// </summary>
        public static Bitlogic.EndianBitConverter DefaultBitConverter { get { return BigEndianBitConverter; } }
    }
}
