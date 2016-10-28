using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core.Interfaces;

namespace NetCheatX.UI.Types
{
    public class WindowItem : EventArgs
    {
        /// <summary>Parent IPluginBase.</summary>
        public IPluginBase ParentPlugin { get; set; }

        /// <summary>Path in menustrip.</summary>
        public string Path { get; set; }

        /// <summary>Unique name of XForm to add.</summary>
        public string UniqueName { get; set; }

        /// <summary>Description of XForm. Displayed in a tooltip.</summary>
        public string Description { get; set; }

        /// <summary>Function to call to initialize XForm.</summary>
        public Core.Types.PluginWindowCallback Callback { get; set; }
    }
}
