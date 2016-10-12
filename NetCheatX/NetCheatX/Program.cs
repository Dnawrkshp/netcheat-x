using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCheatX.UI
{
    static class Program
    {
        public static Plugin.Handler pluginHandler;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Plugin.Host pluginHost = new Plugin.Host();
            pluginHandler = new Plugin.Handler();

            pluginHandler.FindPlugins(AppDomain.CurrentDomain.BaseDirectory + "\\Plugins");
            pluginHost.InitializePlugins();

            Controls.SelectCommunicator select = new Controls.SelectCommunicator(pluginHost, pluginHost.Communicators);
            select.ShowDialog();

            if (pluginHost.ActiveCommunicator == null)
                return;

            // Load settings based on selected communicator
            // Load last layout

            Application.Run(new Controls.Display(pluginHost));
        }
    }
}
