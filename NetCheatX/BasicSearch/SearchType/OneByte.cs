using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchType
{
    public class OneByteS : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Single signed byte. -127 to 128.";

        public string Name { get; } = "Int8";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = true;

        public int Alignment { get; } = 1;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex", "Decimal" };

        // Input type
        public Type ParamType { get; } = typeof(sbyte);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[3];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = result.Value[0].ToString("X2");
            columnValues[2] = ((sbyte)result.Value[0]).ToString();
        }

        public void ResultToLegacyCode(out string code, ISearchResult result)
        {
            code = "0 " + result.Address.ToString("X" + (result.Address > uint.MaxValue ? "16" : "8")) + " " + result.Value[0].ToString("X2");
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

    public class OneByteU : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Single unsigned byte. 0 to 255.";

        public string Name { get; } = "UInt8";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = false;

        public int Alignment { get; } = 1;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex", "Decimal" };

        // Input type
        public Type ParamType { get; } = typeof(byte);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[3];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = result.Value[0].ToString("X2");
            columnValues[2] = result.Value[0].ToString();
        }

        public void ResultToLegacyCode(out string code, ISearchResult result)
        {
            code = "0 " + result.Address.ToString("X" + (result.Address > uint.MaxValue ? "16" : "8")) + " " + result.Value[0].ToString("X2");
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
