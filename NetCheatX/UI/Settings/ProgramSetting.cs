using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetCheatX.UI.Settings
{
    public class ProgramSetting
    {
        private string _path = null;

        public List<string> UpdateURLs { get; set; }

        public List<string> PluginPaths { get; set; }

        public ProgramSetting(string path)
        {
            _path = path;
            UpdateURLs = new List<string>();
            PluginPaths = new List<string>();

            // Default plugins path
            PluginPaths.Add("Plugins");

            if (path == null || !File.Exists(path))
                return;

            
        }

        public void Close()
        {

        }
    }
}
