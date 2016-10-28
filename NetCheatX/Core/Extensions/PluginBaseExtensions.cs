using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Extensions
{
    /// <summary>
    /// Extension functions for IPluginBase inherited class instances.
    /// </summary>
    public static class PluginBaseExtensions
    {
        /// <summary>
        /// Returns a Base64String representation of the instance inheriting the <see cref="T:NetCheatX.Core.Interfaces.IPluginBase"/> interface.
        /// </summary>
        public static string ToBase64String(this Interfaces.IPluginBase pluginBase)
        {
            if (pluginBase == null)
                return null;

            byte[] bytes = Encoding.ASCII.GetBytes(pluginBase.Name + pluginBase.Version + pluginBase.Author);
            return Convert.ToBase64String(bytes);
        }
    }
}
