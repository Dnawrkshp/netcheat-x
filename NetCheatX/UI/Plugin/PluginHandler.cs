using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NetCheatX.Core;

namespace NetCheatX.UI.Plugin
{
    public class PluginHandler
    {

        private List<AvailablePlugin> colAvailablePlugins = new List<AvailablePlugin>();

        // A Collection of all loaded plugins
        public List<AvailablePlugin> AvailablePlugins
        {
            get { return colAvailablePlugins; }
            set { colAvailablePlugins = value; }
        }

        // Searches the Application's Startup Directory for plugins
        public void FindPlugins()
        {
            FindPlugins(AppDomain.CurrentDomain.BaseDirectory);
        }

        // Searches for plugins in given path
        public void FindPlugins(string Path)
        {
            string[] fileNames = new string[0];

            // First empty the collection, we're reloading them all
            colAvailablePlugins.Clear();

            // Go through all the files in the plugin directory
            foreach (string fileOn in Directory.GetFiles(Path, (IntPtr.Size == 8) ? "*.dll64" : "*.dll32", SearchOption.AllDirectories))
            {
                try
                {
                    AddPlugin(fileOn);
                    Array.Resize(ref fileNames, fileNames.Length + 1);
                    fileNames[fileNames.Length - 1] = System.IO.Path.GetFileName(fileOn);
                }
                catch (Exception e)
                {
                    Program.logger.LogException(e);
                    System.Windows.Forms.MessageBox.Show(e.StackTrace + "\r\n\r\n" + e.Message, e.Source);
                }
            }
        }

        private void AddPlugin(string FileName)
        {
            // Create a new assembly from the plugin file we're adding..
            Assembly pluginAssembly = Assembly.LoadFrom(FileName);

            // Loop through all the Types found in the assembly
            foreach (Type pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.IsPublic) // Only look at public types
                {
                    if (!pluginType.IsAbstract)  // Only look at non-abstract types
                    {
                        //  Gets a type object of the interface we need the plugins to match
                        Type typeInterface = pluginType.GetInterface("NetCheatX.Core.Interfaces.IPluginMain", true);

                        // Make sure the interface we want to use actually exists
                        if (typeInterface != null)
                        {
                            try
                            {

                                // Create a new available plugin
                                AvailablePlugin newPlugin = new AvailablePlugin();

                                // Set the assembly path
                                newPlugin.AssemblyPath = FileName;

                                // Create a new instance and store the instance in the collection for later use
                                newPlugin.Instance = (Core.Interfaces.IPluginBase)Activator.CreateInstance(pluginAssembly.GetType(pluginType.ToString()));

                                // Add the new plugin to our collection
                                colAvailablePlugins.Add(newPlugin);

                                // Cleanup a bit
                                newPlugin = null;
                            }
                            catch (Exception e)
                            {
                                Program.logger.LogException(e);

                                // Spit out plugin loading error
                                System.Windows.Forms.MessageBox.Show(e.Message, e.Source);
                            }
                        }

                        typeInterface = null; // Clean			
                    }
                }
            }

            pluginAssembly = null; // More cleanup
        }

    }

    // Data Class for Available Plugin.  Holds an instance of the loaded plugin, as well as the plugin's assembly path
    public class AvailablePlugin
    {
        private Core.Interfaces.IPluginBase myInstance = null;
        private string myAssemblyPath = "";

        public Core.Interfaces.IPluginBase Instance
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
