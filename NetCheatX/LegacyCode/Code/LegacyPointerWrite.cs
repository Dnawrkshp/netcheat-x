using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class LegacyPointerWrite : ILegacyType
    {
        private ulong _address = 0;
        private ulong _offset = 0;
        private bool _is64Bit = false;
        private bool _writeArg2 = false;

        private ILegacyType subCode = null;

        public char Type { get; } = '6';


        public void SetArg1(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _address = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);

            // If address is 64 bit, set is64Bit to true
            _is64Bit = value.Length >= 8;
        }

        public void SetArg2(byte[] value)
        {
            // Ensure array is large enough to convert to ulong
            byte[] tmp = new byte[8];
            Array.Copy(value, 0, tmp, value.Length <= 8 ? (8 - value.Length) : 0, value.Length > 8 ? 8 : value.Length);

            _offset = Globals.BigEndianBitConverter.ToUInt64(tmp, 0);
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

            if (words[0].Length > 1 && words[0][1] != '0')
                _writeArg2 = true;

            try
            {
                // Load address
                _address = Convert.ToUInt64(words[1], 16);

                // If address is 64 bit, set is64Bit to true
                _is64Bit = words[1].Length >= 16;

                // Load offset
                _offset = Convert.ToUInt64(words[2], 16);

                // Load next code and update arg1s
                index++;
                subCode = Processor.GetILegacyType(lines[index][0]);
                if (subCode == null)
                    return false;

                if (!subCode.FromString(host, ref index, lines))
                    return false;
            }
            catch { return false; }

            return true;
        }

        public bool Execute(IPluginHost host)
        {
            if (subCode == null)
                return false;

            try
            {
                byte[] ptr = new byte[_is64Bit ? 8 : 4];
                host.GetMemory(_address, ref ptr);
                ulong ptrAddress = _is64Bit ? host.PlatformBitConverter.ToUInt64(ptr, 0) : (ulong)host.PlatformBitConverter.ToUInt32(ptr, 0);
                ptrAddress += _offset;

                if (_writeArg2)
                    subCode.SetArg2(Globals.BigEndianBitConverter.GetBytes(ptrAddress));
                else
                    subCode.SetArg1(Globals.BigEndianBitConverter.GetBytes(ptrAddress));
            }
            catch { return false; }

            return subCode.Execute(host);
        }

        public void Dispose()
        {
            if (subCode != null)
            {
                subCode.Dispose();
                subCode = null;
            }
        }
    }
}
