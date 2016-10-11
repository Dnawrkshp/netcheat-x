using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class LegacyTextWrite : ILegacyType
    {
        private ulong _address = 0;
        private byte[] _value = null;

        public char Type { get; } = '1';


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
            string line = lines[index];
            string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 3)
                return false;

            if (!Processor.IsValidHex(words[1]))
                return false;

            try
            {
                // Load address
                _address = Convert.ToUInt64(words[1], 16);

                // Load write bytes
                if (words[0].Length > 1 && words[0][1] == '1')
                    _value = Encoding.UTF8.GetBytes(words[2]);
                else
                    _value = Encoding.ASCII.GetBytes(words[2]);
            }
            catch { return false; }

            if (_value == null || _value.Length == 0)
                return false;

            return true;
        }

        public bool Execute(IPluginHost host)
        {
            if (_value == null || _value.Length == 0)
                return false;

            return host.SetMemory(_address, _value);
        }

        public void Dispose()
        {
            _value = null;
        }
    }
}
