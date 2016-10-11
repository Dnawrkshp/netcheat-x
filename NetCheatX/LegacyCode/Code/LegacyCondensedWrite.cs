using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class LegacyCondensedWrite : ILegacyType
    {
        private ulong _address = 0;
        private byte[] _value = null;
        private ulong _increment = 0;
        private ulong _count = 0;

        public char Type { get; } = '4';


        public void SetArg1(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _address = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);
        }

        public void SetArg2(byte[] value)
        {
            _value = value;
        }

        public void SetArg0(byte[] value)
        {

        }

        public bool FromString(IPluginHost host, ref int index, string[] lines)
        {
            // We need a line after this for a valid condensed write code
            if ((index + 1) >= lines.Length)
                return false;

            string line = lines[index];
            string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 3)
                return false;

            if (!Processor.IsValidHex(words[1]) || !Processor.IsValidHex(words[2]))
                return false;

            try
            {
                // Load address
                _address = Convert.ToUInt64(words[1], 16);

                // Load write bytes
                _value = Globals.BigEndianBitConverter.GetBytes(words[2]);
            }
            catch { return false; }

            // Load next line's arguments
            index++;
            line = lines[index];
            words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (line[0] != '4')
                return false;

            if (words.Length < 3)
                return false;

            if (!Processor.IsValidHex(words[1]) || !Processor.IsValidHex(words[2]))
                return false;

            try
            {
                // Load address
                _increment = Convert.ToUInt64(words[1], 16);

                // Load write bytes
                _count = Convert.ToUInt64(words[2], 16);
            }
            catch { return false; }

            return true;
        }

        public bool Execute(IPluginHost host)
        {
            if (_value == null || _value.Length == 0)
                return false;

            try
            {
                for (ulong c = 0; c < _count; c++)
                    host.SetMemory(_address + (_increment * c), _value);
            }
            catch { return false; }

            return true;
        }

        public void Dispose()
        {
            _value = null;
        }
    }
}
