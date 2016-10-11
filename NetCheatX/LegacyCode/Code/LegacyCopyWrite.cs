using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class LegacyCopyWrite : ILegacyType
    {
        private ulong _source = 0;
        private ulong _destination = 0;
        private ulong _size = 0;

        public char Type { get; } = 'F';


        public void SetArg1(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _source = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);
        }

        public void SetArg2(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _destination = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);
        }

        public void SetArg0(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _size = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);
        }

        public bool FromString(IPluginHost host, ref int index, string[] lines)
        {
            string arg0;
            string line = lines[index];
            string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 3)
                return false;

            if (!Processor.IsValidHex(words[1]) || !Processor.IsValidHex(words[2]))
                return false;

            if (words[0].Length <= 1)
                return false;

            arg0 = words[0].Substring(1);

            try
            {
                // Load source address
                _source = Convert.ToUInt64(words[1], 16);

                // Load destination address
                _destination = Convert.ToUInt64(words[2], 16);

                // Load size of copy
                _size = Convert.ToUInt64(arg0, 16);
            }
            catch { return false; }

            return true;
        }

        public bool Execute(IPluginHost host)
        {
            if (_source == _destination || _size == 0)
                return false;

            byte[] buffer = new byte[_size];
            if (host.GetMemory(_source, ref buffer))
                return host.SetMemory(_destination, buffer);

            return false;
        }

        public void Dispose()
        {
            
        }
    }
}
