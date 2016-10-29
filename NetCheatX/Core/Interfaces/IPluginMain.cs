using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// Main plugin interface that NetCheat X instantiates on plugin load. Instantiated and load all subplugins into IPluginHost instance here.
    /// </summary>
    public interface IPluginMain : IPluginBase
    {
        /// <summary>
        /// List of all supported platforms in abbreviated format (PS3, PC, iOS, Android)
        /// Leave null if all are supported
        /// </summary>
        string[] SupportedPlatforms { get; }
    }
}
