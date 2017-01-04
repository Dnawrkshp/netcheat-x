using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// Add on plugin interface.
    /// </summary>
    public interface IAddOn : IPluginBase
    {
        /// <summary>
        /// List of all supported platforms in abbreviated format (PS3, PC, iOS, Android).
        /// Leave null if all are supported.
        /// </summary>
        string[] SupportedPlatforms { get; }

        /// <summary>
        /// Initializes a new XForm identified by param uniqueName.
        /// </summary>
        /// <param name="xForm">Uninitialized XForm.</param>
        /// <param name="uniqueName">XForm identifier.</param>
        /// <returns>False if failed.</returns>
        bool InitializeXForm(out NetCheatX.Core.UI.XForm xForm, string uniqueName);

        /// <summary>
        /// Event triggered when Context Menu is shown with <see cref="NetCheatX.Core.Extensions.ContextMenuStripExtensions.ShowX"/>.
        /// </summary>
        /// <param name="tags">An array of tags separated by spaces indicating the purpose of the data.</param>
        /// <param name="supportedOutputTypes">An array of types indicating the possible arguments.</param>
        /// <returns>Array of items to be added to the context menu.</returns>
        Types.ContextMenuPath[] OnXShow(string[] tags, Type[] supportedOutputTypes);
    }
}
