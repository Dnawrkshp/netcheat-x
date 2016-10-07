using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Search method plugin interface
    /// </summary>
    public interface ISearchMethod
    {
        /// <summary>
        /// Name of the ISearchMethod
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the ISearchMethod's purpose
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the ISearchMethod
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the ISearchMethod
        /// </summary>
        string Version { get; }

        /// <summary>
        /// List of all supported platforms in abbreviated format (PS3, PC, iOS, Android)
        /// Leave null if all are supported
        /// </summary>
        string[] SupportedPlatforms { get; }

        /// <summary>
        /// Whether the ISearchMethod can be used for the first scan, later scans, or all scans
        /// </summary>
        Types.SearchMode Mode { get; }

        /// <summary>
        /// All user entered parameters required by the ISearchMethod
        /// </summary>
        Types.SearchParam[] Params { get; }

        /// <summary>
        /// First scan
        /// </summary>
        /// <param name="result">List of search results. Populated by the scan method</param>
        /// <param name="args">All arguments in order. Defined by ISearchMethod's Params property</param>
        /// <param name="range">Memory regions to be scanned</param>
        void FirstScan(out List<ISearchResult> result, object[] args, Types.MemoryRange[] range);

        /// <summary>
        /// Next scan
        /// </summary>
        /// <param name="result">List of search results from last scan.</param>
        /// <param name="args">All arguments in order. Defined by ISearchMethod's Params property</param>
        void NextScan(ref List<ISearchResult> result, object[] args);

        /// <summary>
        /// Support selected ISearchType
        /// </summary>
        /// <param name="sType">ISearchType to check</param>
        /// <returns>True if ISearchMethod supports ISearchType. False if otherwise</returns>
        bool SupportSearchType(ISearchType sType);

        /// <summary>
        /// Called when the search UI is loaded (not when the scan begins)
        /// </summary>
        void Initialize(IPluginHost host);

        /// <summary>
        /// Called when search UI is unloading
        /// </summary>
        void Dispose(IPluginHost host);
    }
}
