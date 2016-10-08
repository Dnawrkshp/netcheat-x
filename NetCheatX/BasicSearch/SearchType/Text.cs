using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchType
{
    public class Text : ISearchType
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Text string.";

        public string Name { get; } = "Text";

        public string Version { get; } = "1.0";

        public bool Signed { get; } = false;

        public int Alignment { get; } = 1;

        // Columns
        public string[] Columns { get; } = new string[] { "Address", "Text" };

        // Input type
        public Type ParamType { get; } = typeof(string);

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;

        public void ProcessResult(out string[] columnValues, ISearchResult result)
        {
            columnValues = new string[2];

            columnValues[0] = result.Address.ToString("X16");
            columnValues[1] = Encoding.UTF8.GetString(result.Value);
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
