using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchType
{
    public class XByteS : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "X signed bytes. Number of bytes determined by number of input bytes";

        public string Name { get; } = "X Byte Signed";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = true;

        public int Alignment { get; } = 1;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex" };

        // Input type
        public Type ParamType { get; } = typeof(sbyte[]);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[2];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
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

    public class XByteU : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "X unsigned bytes. Number of bytes determined by number of input bytes";

        public string Name { get; } = "X Byte Unsigned";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = false;

        public int Alignment { get; } = 1;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex" };

        // Input type
        public Type ParamType { get; } = typeof(byte[]);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[2];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
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
