using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core.Interfaces;

namespace NetCheatX.UI.Types
{
    public class FunctionItem : EventArgs
    {
        /// <summary>Parent IPluginBase.</summary>
        public IPluginBase ParentPlugin { get; set; }

        /// <summary>Path in menustrip.</summary>
        public string Path { get; set; }

        /// <summary>Icon for toolstrip</summary>
        public System.Drawing.Image Icon { get; set; }

        /// <summary>Description of Function. Displayed in a tooltip.</summary>
        public string Description { get; set; }

        /// <summary>Function to call.</summary>
        public Core.Types.PluginFunctionCallback Callback { get; set; }
    }
}
