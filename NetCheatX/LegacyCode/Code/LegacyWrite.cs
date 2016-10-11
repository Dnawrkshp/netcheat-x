using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class LegacyWrite : ILegacyType
    {
        private ulong _address = 0;
        private byte[] _value = null;
        private byte _type = 0;

        public char Type { get; } = '0';


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

            if (!Processor.IsValidHex(words[1]) || !Processor.IsValidHex(words[2]))
                return false;

            try
            {
                // Load address
                _address = Convert.ToUInt64(words[1], 16);

                // Load write bytes
                _value = Globals.BigEndianBitConverter.GetBytes(words[2]);

                // Load write type
                _type = 0;
                if (words[0].Length > 1)
                    _type = (byte)(words[0][0] - 0x30);
            }
            catch { return false; }

            if (_value == null || _value.Length == 0)
                return false;

            return true;
        }

        public bool Execute(IPluginHost host)
        {
            byte[] get;
            int index;

            if (_value == null || _value.Length == 0)
                return false;

            if (_type == 0)
                return host.SetMemory(_address, _value);

            get = new byte[_value.Length];
            host.GetMemory(_address, ref get);
            switch (_type)
            {
                case 1: // OR
                    for (index = 0; index < get.Length; index++)
                        get[index] |= _value[index];
                    break;
                case 2: // AND
                    for (index = 0; index < get.Length; index++)
                        get[index] &= _value[index];
                    break;
                case 3: // XOR
                    for (index = 0; index < get.Length; index++)
                        get[index] ^= _value[index];
                    break;
                default:
                    return false;
            }

            return host.SetMemory(_address, get);
        }

        public void Dispose()
        {
            _value = null;
        }
    }
}
