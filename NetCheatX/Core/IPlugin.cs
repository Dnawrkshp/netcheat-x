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
        /// List of all supported platforms in abbreviated format (PS3, PC, iOS, Android)
        /// Leave null if all are supported
        /// </summary>
        string[] SupportedPlatforms { get; }

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
