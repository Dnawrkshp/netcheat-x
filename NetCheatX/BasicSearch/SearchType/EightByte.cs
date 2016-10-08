using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchType
{
    public class EightByteS : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "8 signed bytes. -9223372036854775808 to 9223372036854775807.";

        public string Name { get; } = "Int64";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = true;

        public int Alignment { get; } = 8;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex", "Decimal" };

        // Input type
        public Type ParamType { get; } = typeof(long);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[3];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
            columnValues[2] = _host.PlatformBitConverter.ToInt64(result.Value, 0).ToString();
        }

        public void ResultToLegacyCode(out string code, ISearchResult result)
        {
            code = "0 " + result.Address.ToString("X" + (result.Address > uint.MaxValue ? "16" : "8")) + " " + _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
        }

        public void Initialize(IPluginHost host)
        {
            _host = host;
        }

        public void Dispose(IPluginHost host)
        {
            _host = null;
        }
    }

    public class EightByteU : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "8 unsigned bytes. 0 to 18446744073709551616.";

        public string Name { get; } = "UInt64";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = false;

        public int Alignment { get; } = 8;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex", "Decimal" };

        // Input type
        public Type ParamType { get; } = typeof(ulong);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[3];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
            columnValues[2] = _host.PlatformBitConverter.ToUInt64(result.Value, 0).ToString();
        }

        public void ResultToLegacyCode(out string code, ISearchResult result)
        {
            code = "0 " + result.Address.ToString("X" + (result.Address > uint.MaxValue ? "16" : "8")) + " " + _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
        }

        public void Initialize(IPluginHost host)
        {
            _host = host;
        }

        public void Dispose(IPluginHost host)
        {
            _host = null;
        }
    }
}
