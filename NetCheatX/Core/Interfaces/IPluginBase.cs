using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// Basic properties and methods of all plugins.
    /// </summary>
    public interface IPluginBase
    {
        /// <summary>
        /// Name of the Plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the Plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the Plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the Plugin.
        /// </summary>
        ObjectVersion Version { get; }

        /// <summary>
        /// Called as UI is loading.
        /// </summary>
        void Initialize(IPluginHost host);

        /// <summary>
        /// Called UI is unloading.
        /// </summary>
        void Dispose(IPluginHost host);
    }
}
