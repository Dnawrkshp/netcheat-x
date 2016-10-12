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

        private List<Core.UI.XForm> _xForms = new List<Core.UI.XForm>();

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

            // Load existing settings
            host.PlatformSettings = new Settings.PlatformSetting(host, "C:/Users/Dan/Desktop/ps3.txt", host.ActiveCommunicator.Platform);

            // Update dock panel theme
            switch (host.PlatformSettings.Theme)
            {
                case Core.Types.MetroTheme.Blue:
                    dockPanel.Theme = _blueTheme;
                    break;
                case Core.Types.MetroTheme.Dark:
                    dockPanel.Theme = _darkTheme;
                    break;
                case Core.Types.MetroTheme.Light:
                    dockPanel.Theme = _lightTheme;
                    break;
            }

            // Add host events
            host.MenuItemAdded += Host_MenuItemAdded;
            host.WindowItemAdded += Host_WindowItemAdded;

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

        #region Host Events

        // Add WindowItem to MenuStrip
        private void Host_WindowItemAdded(object sender, Core.Types.WindowItem e)
        {
            if (e.callback == null || e.uniqueName == null || e.uniqueName == "")
                return;

            if (e.codeEditor != null) // Add to /View/Editors/
                AddMenuStripItem("View/Editors/" + e.path, e.uniqueName, e.description, e.callback);
            else if (e.addOn != null) // Add to /View/
                AddMenuStripItem("View/" + e.path, e.uniqueName, e.description, e.callback);
            else if (e.communicator != null) // Add to /View/Communicator
                AddMenuStripItem("View/Communicator/" + e.path, e.uniqueName, e.description, e.callback);
        }

        private void Host_MenuItemAdded(object sender, Core.Types.MenuItem e)
        {
            
        }

        private void AddMenuStripItem(string path, string uniqueName, string tooltip, NetCheatX.Core.Types.AddXFormCallback callback)
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
                        _xForms.Add(xform);
                        UpdateTheme(_host.PlatformSettings.Theme, xform);
                        xform.Show(dockPanel);
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

        #region Theme

        // Update theme of all or specific docked XForm(s)
        private void UpdateTheme(Core.Types.MetroTheme theme, Core.UI.XForm xform = null)
        {
            Color back = Color.Black, fore = Color.Black;

            switch (theme)
            {
                case Core.Types.MetroTheme.Blue:
                    back = _blueTheme.ColorPalette.MainWindowActive.Background;
                    fore = _blueTheme.ColorPalette.TabUnselected.Text;
                    break;
                case Core.Types.MetroTheme.Dark:
                    back = _darkTheme.ColorPalette.MainWindowActive.Background;
                    fore = _darkTheme.ColorPalette.TabUnselected.Text;
                    break;
                case Core.Types.MetroTheme.Light:
                    back = _lightTheme.ColorPalette.MainWindowActive.Background;
                    fore = _lightTheme.ColorPalette.TabUnselected.Text;
                    break;
            }

            if (xform != null)
                xform.SetTheme(_host.PlatformSettings.Theme, back, fore);
            else  if (_xForms != null)
            {
                foreach (Core.UI.XForm xf in _xForms)
                    xf.SetTheme(_host.PlatformSettings.Theme, back, fore);
            }
        }

        #endregion

    }
}
