using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class LegacyCopyPaste : ILegacyType
    {
        private static byte[] _copiedBytes = null;
        private ulong _address = 0;
        private ulong _size = 0;
        private byte _type = 0;

        public char Type { get; } = 'A';


        public void SetArg1(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _address = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);
        }

        public void SetArg2(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _size = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);
        }

        public void SetArg0(byte[] value)
        {
            if (value == null || value.Length == 0)
                return;

            _type = value[0];
        }

        public bool FromString(IPluginHost host, ref int index, string[] lines)
        {
            string line = lines[index];
            string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 3)
                return false;

            if (!Processor.IsValidHex(words[1]) || !Processor.IsValidHex(words[2]))
                return false;

            if (words[0].Length <= 1)
                return false;

            try
            {
                // Load type of operation (1=copy,2=paste)
                _type = byte.Parse(words[0].Substring(1)[0].ToString());

                // Load source address
                _address = Convert.ToUInt64(words[1], 16);

                // Load destination address
                _size = Convert.ToUInt64(words[2], 16);
            }
            catch { return false; }

            return true;
        }

        public bool Execute(IPluginHost host)
        {
            if (_size == 0 || (_type != 1 && _type != 2))
                return false;

            if (_type == 1) // copy
            {
                _copiedBytes = new byte[_size];
                host.GetMemory(_address, ref _copiedBytes);
            }
            else if (Type == 2) // paste
            {
                host.SetMemory(_address, _copiedBytes);
            }

            return false;
        }

        public void Dispose()
        {
            _copiedBytes = null;
        }
    }
}
