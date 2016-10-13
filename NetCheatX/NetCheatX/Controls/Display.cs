using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MetroFramework.Forms;
using MetroFramework.Controls;

namespace NetCheatX.UI.Controls
{
    public partial class Display : Form
    {

        private Plugin.Host _host = null;

        #region Theme Variables

        private VS2015BlueTheme _blueTheme = new VS2015BlueTheme();
        private VS2015DarkTheme _darkTheme = new VS2015DarkTheme();
        private VS2015LightTheme _lightTheme = new VS2015LightTheme();

        #endregion

        public Display(Plugin.Host host)
        {
            _host = host;

            // Need an instance of Plugin.Host to function
            if (_host == null)
            {
                Close();
                return;
            }

            InitializeComponent();

            // Setup dock panel
            host.XFormDockPanel = dockPanel;

            // Set form Text
            UpdateTextStatus("Not Ready");

            // Load existing settings
            host.PlatformSettings = new Settings.PlatformSetting(host, System.IO.Path.Combine(Application.StartupPath, "Settings", host.ActiveCommunicator.Platform), host.ActiveCommunicator.Platform);

            // Add host events
            host.MenuItemAdded += Host_MenuItemAdded;
            host.WindowItemAdded += Host_WindowItemAdded;

            host.ActiveCommunicator.ReadyChanged += ActiveCommunicator_ReadyChanged;

            // Set toolstrip invisible until it an item is added to it
            toolStrip.Visible = false;

            // Trigger events for all UI items in Host
            foreach (Core.Types.AddCodeItem item in host.AddCodeItems)
                host.TriggerCodeItemAdded(item);

            foreach (Core.Types.WindowItem item in host.WindowItems)
                host.TriggerWindowItemAdded(item);

            foreach (Core.Types.MenuItem item in host.MenuItems)
                host.TriggerMenuItemAdded(item);
        }

        #region Host Event Handlers

        // Update Ready state
        private void ActiveCommunicator_ReadyChanged(object sender, string e)
        {
            Invoke((MethodInvoker)delegate
            {
                UpdateTextStatus(e);
            });
        }

        // Add WindowItem to MenuStrip
        private void Host_WindowItemAdded(object sender, Core.Types.WindowItem e)
        {
            if (e.callback == null || e.uniqueName == null || e.uniqueName == "" || e.pluginExtension == null)
                return;

            if (e.pluginExtension is Core.ICodeEditor) // Add to /View/Editors/
                AddMenuStripItem("View/Editors/" + e.path, e.uniqueName, e.description, e.pluginExtension, e.callback);
            else if (e.pluginExtension is Core.IAddOn) // Add to /View/
                AddMenuStripItem("View/" + e.path, e.uniqueName, e.description, e.pluginExtension, e.callback);
            else if (e.pluginExtension is Core.ICommunicator) // Add to /View/Communicator
                AddMenuStripItem("View/Communicator/" + e.path, e.uniqueName, e.description, e.pluginExtension, e.callback);
        }

        private void Host_MenuItemAdded(object sender, Core.Types.MenuItem e)
        {
            
        }

        private void AddMenuStripItem(string path, string uniqueName, string tooltip, Core.IPluginExtension parentPlugin, NetCheatX.Core.Types.AddXFormCallback callback)
        {
            string[] pathParts = path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int partIndex = 1;
            ToolStripMenuItem parent = null;

            if (pathParts.Length == 0)
                return;

            // Find first parent menu item
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                if (item.Text.ToLower() == pathParts[0].ToLower())
                {
                    parent = item;
                    break;
                }
            }

            // If none found then add it
            if (parent == null)
            {
                parent = new ToolStripMenuItem(pathParts[0]);
                menuStrip.Items.Add(parent);
            }

            // Loop through recursively adding/finding each menu item of path
            while (partIndex < pathParts.Length)
                parent = RecursiveAddToolStripItem(pathParts, ref partIndex, parent);

            parent.ToolTipText = tooltip;
            parent.Click += (sender, e) =>
            {
                Core.UI.XForm xform;
                if (callback.Invoke(out xform, _host))
                {
                    if (xform != null)
                    {
                        xform.UniqueName = uniqueName;
                        xform.ParentPlugin = parentPlugin;
                        _host.XForms.Add(xform);
                        xform.Show(_host.XFormDockPanel);
                    }
                }
            };
        }

        private ToolStripMenuItem RecursiveAddToolStripItem(string[] pathParts, ref int index, ToolStripMenuItem parent)
        {
            ToolStripMenuItem child = null;

            // If parent has no drop down items to search through, add the item
            if (parent.DropDownItems == null || parent.DropDownItems.Count == 0)
                goto skip;

            // Search for item within parent DropDownItems
            foreach (ToolStripMenuItem item in parent.DropDownItems)
            {
                if (item.Text.ToLower() == pathParts[index].ToLower())
                {
                    child = item;
                    index++;
                    return child;
                }
            }

            skip:;

            // Not found so add the item
            child = new ToolStripMenuItem(pathParts[index]);
            parent.DropDownItems.Add(child);
            index++;

            return child;
        }

        #endregion

        #region Other Event Handlers

        private void aboutNetCheatXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Display_FormClosing(object sender, FormClosingEventArgs e)
        {
            _host.Dispose();
        }

        #endregion

        private void UpdateTextStatus(string status)
        {
            Text = "NetCheatX " + _host.ActiveCommunicator.Platform + " [" + status + "]";
        }
    }
}
