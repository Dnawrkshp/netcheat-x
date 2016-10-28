using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// Search method plugin interface
    /// </summary>
    public interface ISearchMethod : IPluginBase
    {
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
        /// <param name="searchType">ISearchType in use</param>
        /// <param name="args">All arguments in order. Defined by ISearchMethod's Params property</param>
        /// <param name="range">Memory regions to be scanned</param>
        /// <param name="setProgress">Callback to set the progress of scan</param>
        void FirstScan(out List<ISearchResult> result, ISearchType searchType, object[] args, Types.MemoryRange[] range, Types.SetProgressCallback setProgress);

        /// <summary>
        /// Next scan
        /// </summary>
        /// <param name="result">List of search results from last scan.</param>
        /// <param name="searchType">ISearchType in use</param>
        /// <param name="args">All arguments in order. Defined by ISearchMethod's Params property</param>
        /// <param name="setProgress">Callback to set the progress of scan</param>
        void NextScan(ref List<ISearchResult> result, ISearchType searchType, object[] args, Types.SetProgressCallback setProgress);

        /// <summary>
        /// Support selected ISearchType
        /// </summary>
        /// <param name="sType">ISearchType to check</param>
        /// <returns>True if ISearchMethod supports ISearchType. False if otherwise</returns>
        bool SupportSearchType(ISearchType sType);
    }
}
