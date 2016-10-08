using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchMethod
{
    class NotEqualTo : ISearchMethod
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "If parameter0 bytes are not equal to bytes within memory range.";

        public string Name { get; } = "Not Equal To";

        public string Version { get; } = "1.0";

        public Types.SearchMode Mode { get; } = Types.SearchMode.Both;

        public Types.SearchParam[] Params { get; } = new Types.SearchParam[] {
            // Set process to true so the ISearchType processes the input into a byte array
            // Set type to null for good measure
            new Types.SearchParam() { name = "parameter0", process = true, type = null, description = "Value to skip in search." }
        };

        // Support all platforms
        public string[] SupportedPlatforms { get; } = null;


        public void FirstScan(out List<ISearchResult> result, ISearchType searchType, object[] args, Types.MemoryRange[] range)
        {
            // Initialize list
            result = new List<ISearchResult>();
            if (_host == null)
                return;

            // Grab parameters
            byte[] param0 = (byte[])args[0];

            // Loop through each range and scan
            foreach (Types.MemoryRange r in range)
            {
                Search.FirstScan(_host, this, r, ref result, Search.SearchType.NotEqualTo, false, searchType.Alignment, param0);
            }
        }

        public void NextScan(ref List<ISearchResult> result, ISearchType searchType, object[] args)
        {
            if (_host == null)
                return;

            // Grab parameters
            byte[] param0 = (byte[])args[0];

            // Perform scan
            Search.NextScan(_host, this, ref result, Search.SearchType.NotEqualTo, false, param0);
        }

        public bool SupportSearchType(ISearchType sType)
        {
            // Not Equal To should support all search types
            return true;
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
