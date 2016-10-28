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

using NetCheatX.Core.Interfaces;

namespace NetCheatX.UI.Controls
{
    public partial class Display : Form
    {

        private Plugin.PluginHost _host = null;

        public Display(Plugin.PluginHost host)
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
            host.FunctionItemAdded += Host_FunctionItemAdded;
            host.WindowItemAdded += Host_WindowItemAdded;

            host.ActiveCommunicator.ReadyChanged += ActiveCommunicator_ReadyChanged;

            // Set toolstrip invisible until it an item is added to it
            toolStrip.Visible = false;

            // Trigger events for all UI items in Host
            foreach (Types.WindowItem item in host.WindowItems)
                host.TriggerWindowItemAdded(item);

            foreach (Types.FunctionItem item in host.FunctionItems)
                host.TriggerFunctionItemAdded(item);
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
        private void Host_WindowItemAdded(object sender, Types.WindowItem e)
        {
            if (e.Callback == null || e.UniqueName == null || e.UniqueName == "" || e.ParentPlugin == null)
                return;

            if (e.ParentPlugin is ICodeEditor) // Add to /View/Code Editors/
                AddMenuStripItem("View/Code Editors/" + e.Path, e);
            else if (e.ParentPlugin is IAddOn) // Add to /View/Add Ons
                AddMenuStripItem("View/Add Ons" + e.Path, e);
            else if (e.ParentPlugin is ICommunicator) // Add to /View/Communicator
                AddMenuStripItem("View/Communicator/" + e.Path, e);
            else if (e.ParentPlugin is ISearchType) // Add to /View/Search Types
                AddMenuStripItem("View/Search Types/" + e.Path, e);
            else if (e.ParentPlugin is ISearchMethod) // Add to /View/Search Methods
                AddMenuStripItem("View/Search Methods/" + e.Path, e);
            else if (e.ParentPlugin is ITypeEditor) // Add to /View/Type Editors
                AddMenuStripItem("View/Type Editors/" + e.Path, e);
            else if (e.ParentPlugin is IPluginBase) // Add to /View/Plugins
                AddMenuStripItem("View/Plugins/" + e.Path, e);
        }

        private void Host_FunctionItemAdded(object sender, Types.FunctionItem e)
        {
            
        }

        private void AddMenuStripItem(string path, Types.WindowItem windowItem)
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

            parent.ToolTipText = windowItem.Description;
            parent.Click += (sender, e) =>
            {
                Core.UI.XForm xform;
                if (windowItem.Callback.Invoke(out xform, _host))
                {
                    if (xform != null)
                    {
                        xform.UniqueName = windowItem.UniqueName;
                        xform.ParentPlugin = windowItem.ParentPlugin;
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

        #region Private Functions

        private void UpdateTextStatus(string status)
        {
            Text = "NetCheatX " + _host.ActiveCommunicator.Platform + " [" + status + "]";
        }

        #endregion

    }
}
