using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// UI application search interface
    /// </summary>
    public interface ISearchMethodHost
    {
        /// <summary>
        /// Register search method with Search UI
        /// </summary>
        /// <param name="searchMethod">Search method to add</param>
        /// <returns>True if succeeded. False if ISearchMethod with name already exists</returns>
        bool RegisterSearchMethod(ISearchMethod searchMethod);
    }
}
