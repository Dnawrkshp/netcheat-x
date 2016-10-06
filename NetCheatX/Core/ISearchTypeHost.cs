using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Search type plugin interface
    /// </summary>
    public interface ISearchTypeHost
    {
        /// <summary>
        /// Register search type with Search UI
        /// </summary>
        /// <param name="searchType">Search type to add</param>
        /// <returns>True if succeeded. False if ISearchMethod with name already exists</returns>
        bool RegisterSearchType(ISearchType searchType);
    }
}
