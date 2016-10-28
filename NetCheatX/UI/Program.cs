using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCheatX.UI
{
    static class Program
    {
        public static Plugin.PluginHandler pluginHandler;
        public static Settings.Logger logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Setup workspace
            if (!System.IO.Directory.Exists("logs"))
                System.IO.Directory.CreateDirectory("logs");
            if (!System.IO.Directory.Exists("Settings"))
                System.IO.Directory.CreateDirectory("Settings");
            if (!System.IO.Directory.Exists("Plugins"))
                System.IO.Directory.CreateDirectory("Plugins");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Setup variables
            Plugin.PluginHost pluginHost = new Plugin.PluginHost();
            pluginHandler = new Plugin.PluginHandler();
            logger = new Settings.Logger("logs", "NetCheatX", DateTime.Now);

            // Load and initialize plugins
            pluginHandler.FindPlugins(AppDomain.CurrentDomain.BaseDirectory + "\\Plugins");
            pluginHost.InitializePlugins();

            // Load ICommunicator plugin
            Controls.SelectCommunicator select = new Controls.SelectCommunicator(pluginHost, pluginHost.Communicators);
            select.ShowDialog();

            if (pluginHost.ActiveCommunicator == null)
                return;

            // Load UI
            Application.Run(new Controls.Display(pluginHost));

            // Clean up
            pluginHandler.AvailablePlugins.Clear();

            pluginHandler = null;
            pluginHost = null;
            logger = null;
        }
    }
}
