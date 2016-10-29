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

            this.Icon = Properties.Resources.ncx;

            // Setup dock panel
            host.XFormDockPanel = dockPanel;

            // Set form Text
            UpdateTextStatus("Not Ready");

            // Add host events
            host.FunctionItemAdded += Host_FunctionItemAdded;
            host.WindowItemAdded += Host_WindowItemAdded;

            host.AddOns.PluginAdded += Container_PluginAdded;
            host.AddOns.PluginRemoved += Container_PluginRemoved;
            host.CodeEditors.PluginAdded += Container_PluginAdded;
            host.CodeEditors.PluginRemoved += Container_PluginRemoved;
            host.Communicators.PluginAdded += Container_PluginAdded;
            host.Communicators.PluginRemoved += Container_PluginRemoved;
            host.SearchMethods.PluginAdded += Container_PluginAdded;
            host.SearchMethods.PluginRemoved += Container_PluginRemoved;
            host.SearchTypes.PluginAdded += Container_PluginAdded;
            host.SearchTypes.PluginRemoved += Container_PluginRemoved;
            host.TypeEditors.PluginAdded += Container_PluginAdded;
            host.TypeEditors.PluginRemoved += Container_PluginRemoved;

            host.ActiveCommunicator.ReadyChanged += ActiveCommunicator_ReadyChanged;

            // Set toolstrip invisible until it an item is added to it
            toolStrip.Visible = false;

            // Initialize all plugins added to containers
            foreach (IPluginBase p in host.AddOns)
                p.Initialize(_host);
            foreach (IPluginBase p in host.CodeEditors)
                p.Initialize(_host);
            foreach (IPluginBase p in host.Communicators)
                p.Initialize(_host);
            foreach (IPluginBase p in host.SearchMethods)
                p.Initialize(_host);
            foreach (IPluginBase p in host.SearchTypes)
                p.Initialize(_host);
            foreach (IPluginBase p in host.TypeEditors)
                p.Initialize(_host);

            // Load existing settings
            host.PlatformSettings = new Settings.PlatformSetting(host, System.IO.Path.Combine(Application.StartupPath, "Settings", host.ActiveCommunicator.Platform), host.ActiveCommunicator.Platform);
        }

        #region Host Event Handlers

        // Initialize added plugins
        private void Container_PluginAdded(object sender, Core.Types.PluginBaseChangedEventArgs e)
        {
            e.Plugin.Initialize(_host);
        }

        // Dispose removed plugins
        private void Container_PluginRemoved(object sender, Core.Types.PluginBaseChangedEventArgs e)
        {
            e.Plugin.Dispose(_host);
        }

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
                AddMenuStripItem("View/Add Ons/" + e.Path, e);
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
                        if (xform.Icon == null)
                            xform.Icon = Properties.Resources.ncx;
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
            _host.FunctionItemAdded -= Host_FunctionItemAdded;
            _host.WindowItemAdded -= Host_WindowItemAdded;

            _host.AddOns.PluginAdded -= Container_PluginAdded;
            _host.AddOns.PluginRemoved -= Container_PluginRemoved;
            _host.CodeEditors.PluginAdded -= Container_PluginAdded;
            _host.CodeEditors.PluginRemoved -= Container_PluginRemoved;
            _host.Communicators.PluginAdded -= Container_PluginAdded;
            _host.Communicators.PluginRemoved -= Container_PluginRemoved;
            _host.SearchMethods.PluginAdded -= Container_PluginAdded;
            _host.SearchMethods.PluginRemoved -= Container_PluginRemoved;
            _host.SearchTypes.PluginAdded -= Container_PluginAdded;
            _host.SearchTypes.PluginRemoved -= Container_PluginRemoved;
            _host.TypeEditors.PluginAdded -= Container_PluginAdded;
            _host.TypeEditors.PluginRemoved -= Container_PluginRemoved;

            _host.ActiveCommunicator.ReadyChanged -= ActiveCommunicator_ReadyChanged;

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
