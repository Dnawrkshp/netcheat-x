using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Basic properties all plugin extensions should have
    /// </summary>
    public interface IPluginExtension
    {
        /// <summary>
        /// Name of the Plugin Extension
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the Plugin Extension
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the Plugin Extension
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the Plugin Extension
        /// </summary>
        string Version { get; }
    }
}
