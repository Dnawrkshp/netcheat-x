using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchMethod
{
    class ValueBetween : ISearchMethod
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "If values within memory range are between parameter0 and parameter1.";

        public string Name { get; } = "Value Between";

        public string Version { get; } = "1.0";

        public Types.SearchMode Mode { get; } = Types.SearchMode.Both;

        public Types.SearchParam[] Params { get; } = new Types.SearchParam[] {
            // Set process to true so the ISearchType processes the input into a byte array
            // Set type to null for good measure
            new Types.SearchParam() { name = "parameter0", process = true, type = null, description = "Minimum bound of range." },
            new Types.SearchParam() { name = "parameter1", process = true, type = null, description = "Maximum bound of range." }
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
            byte[] param1 = (byte[])args[1];

            // Loop through each range and scan
            foreach (Types.MemoryRange r in range)
            {
                Search.FirstScan(_host, this, r, ref result, Search.SearchType.ValueBetween, searchType.Signed, searchType.Alignment, param0, param1);
            }
        }

        public void NextScan(ref List<ISearchResult> result, ISearchType searchType, object[] args)
        {
            if (_host == null)
                return;

            // Grab parameters
            byte[] param0 = (byte[])args[0];
            byte[] param1 = (byte[])args[1];

            // Perform scan
            Search.NextScan(_host, this, ref result, Search.SearchType.ValueBetween, searchType.Signed, param0, param1);
        }

        public bool SupportSearchType(ISearchType sType)
        {
            // Value Between should support all search types
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
