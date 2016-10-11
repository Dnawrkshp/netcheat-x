using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class LegacyComparisonMaskUnset : ILegacyType
    {
        private ulong _address = 0;
        private byte[] _value = null;
        private List<ILegacyType> _codes = null;

        public char Type { get; } = 'E';


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
                // Load address
                _address = Convert.ToUInt64(words[1], 16);

                // Load compare bytes
                _value = Globals.BigEndianBitConverter.GetBytes(words[2]);

                // Load size
                uint size = Convert.ToUInt32(arg0, 16) + (uint)index;

                // Cleanup old ILegacyType list
                Cleanup();
                _codes = new List<ILegacyType>();

                // Loop through and add each code
                index++;
                for (; index <= size; index++)
                    Processor.AddLegacyCode(host, ref _codes, ref index, lines);
            }
            catch { return false; }

            if (_value == null || _value.Length == 0)
                return false;

            return true;
        }

        public bool Execute(IPluginHost host)
        {
            if (_value == null || _value.Length == 0 || _codes == null || _codes.Count == 0)
                return false;

            // Get memory
            byte[] cmp = new byte[_value.Length];
            host.GetMemory(_address, ref cmp);

            // And all the bytes from _value with cmp
            for (int x = 0; x < cmp.Length; x++)
                cmp[x] &= _value[x];

            // If resulting byte array is equal to _value then execute codes
            if (NetCheatX.Core.Bitlogic.Compare.BAEqual(cmp, _value))
            {
                foreach (ILegacyType code in _codes)
                    code.Execute(host);
            }

            return true;
        }

        // Clean up
        public void Dispose()
        {
            Cleanup();

            _value = null;
        }


        private void Cleanup()
        {
            if (_codes != null)
            {
                for (int x = 0; x < _codes.Count; x++)
                {
                    _codes[x].Dispose();
                    _codes[x] = null;
                }
                _codes.Clear();
                _codes = null;
            }
        }
    }
}
