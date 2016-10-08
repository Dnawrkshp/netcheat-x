using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Plugin interface
    /// Called when plugin DLL is loaded
    /// Used to register other plugins
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Register all plugins with UI
        /// </summary>
        void Initialize(IPluginHost host);

        /// <summary>
        /// Clean up
        /// </summary>
        void Dispose(IPluginHost host);
    }
}
