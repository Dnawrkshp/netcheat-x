using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// UI application type editor interface
    /// </summary>
    public interface ITypeEditorHost
    {
        /// <summary>
        /// Register ITypeEditor with Search UI
        /// </summary>
        /// <param name="typeEditor">ITypeEditor to add</param>
        /// <returns>True if succeeded. False if ITypeEditor with name/version already exists</returns>
        bool RegisterTypeEditor(ITypeEditor typeEditor);
    }
}
