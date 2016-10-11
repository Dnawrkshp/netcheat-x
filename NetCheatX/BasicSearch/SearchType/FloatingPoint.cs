using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchType
{
    public class SingleFP : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Single-precision floating point number. 4 bytes.";

        public string Name { get; } = "Float";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = false;

        public int Alignment { get; } = 4;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex", "Float" };

        // Input type
        public Type ParamType { get; } = typeof(float);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[3];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
            columnValues[2] = _host.PlatformBitConverter.ToSingle(result.Value, 0).ToString("G");
        }

        public void ResultToLegacyCode(out string code, ISearchResult result)
        {
            code = "2 " + result.Address.ToString("X" + (result.Address > uint.MaxValue ? "16" : "8")) + " " + _host.PlatformBitConverter.ToSingle(result.Value, 0).ToString("G");
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

    public class DoubleFP : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Double-precision floating point number. 8 bytes.";

        public string Name { get; } = "Double";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = false;

        public int Alignment { get; } = 8;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Hex", "Double" };

        // Input type
        public Type ParamType { get; } = typeof(double);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[3];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = _host.PlatformBitConverter.ToString(result.Value).Replace("-", "");
            columnValues[2] = _host.PlatformBitConverter.ToDouble(result.Value, 0).ToString("G");
        }

        public void ResultToLegacyCode(out string code, ISearchResult result)
        {
            code = "21 " + result.Address.ToString("X" + (result.Address > uint.MaxValue ? "16" : "8")) + " " + _host.PlatformBitConverter.ToDouble(result.Value, 0).ToString("G");
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
