using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode
{
    public class Plugin : IPlugin
    {
        private LegacyCodeEditor _legacyCodeEditor = null;

        // Register our PC Communicator
        public void Initialize(IPluginHost host)
        {
            host.RegisterCodeEditor((_legacyCodeEditor = new LegacyCode.LegacyCodeEditor()));
        }

        // Clean up
        public void Dispose(IPluginHost host)
        {
            _legacyCodeEditor = null;
        }
    }
}
