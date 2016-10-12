using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace NetCheatX.UI.Plugin
{
    // Data Class for Available Plugin.  Holds an instance of the loaded plugin, as well as the plugin's assembly path
    public class AvailablePlugin
    {
        private IPlugin myInstance = null;
        private string myAssemblyPath = "";

        public IPlugin Instance
        {
            get { return myInstance; }
            set { myInstance = value; }
        }
        public string AssemblyPath
        {
            get { return myAssemblyPath; }
            set { myAssemblyPath = value; }
        }
    }
}
