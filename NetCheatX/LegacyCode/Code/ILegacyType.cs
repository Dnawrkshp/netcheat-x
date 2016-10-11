using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public interface ILegacyType
    {
        // Code type identifier
        char Type { get; }

        // Set arg1 value
        void SetArg1(byte[] value);

        // Set arg2 value
        void SetArg2(byte[] value);

        // Set arg0 value
        void SetArg0(byte[] value);

        // Load code from string at index
        bool FromString(IPluginHost host, ref int index, string[] lines);

        // Execute code
        bool Execute(IPluginHost host);

        // Clean up
        void Dispose();
    }
}
