using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace PCCommunicator
{
    public class Plugin : IPlugin
    {
        private Communicator _communicator;

        // Register our PC Communicator
        public void Initialize(IPluginHost host)
        {
            host.RegisterCommunicator((_communicator = new Communicator()));
        }

        // Clean up
        public void Dispose(IPluginHost host)
        {
            _communicator = null;
        }
    }
}
