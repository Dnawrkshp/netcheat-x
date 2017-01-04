using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core.Interfaces;
using System.Windows.Forms;
using System.ComponentModel;

namespace NetCheatX.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContextMenuStripExtensions
    {
        /// <summary>
        /// This contains a counter to help make names unique.
        /// </summary>
        private static int menuNameCounter = 0;

        /// <summary>
        /// Displays a <see cref="System.Windows.Forms.ContextMenuStrip"/> inheriting control with additional ToolStripItems provided by other plugins.
        /// </summary>
        /// <typeparam name="T">Control that inherits <see cref="System.Windows.Forms.ContextMenuStrip"/>.</typeparam>
        /// <param name="instance">Instance of the context menu.</param>
        /// <param name="newInstance">An empty instance of a control that inherits <see cref="T:System.Windows.Forms.ContextMenuStrip"/>.</param>
        /// <param name="control">The control (typically, a ToolStripDropDownButton) that is the reference point for the ToolStripDropDown position.</param>
        /// <param name="position">The horizontal and vertical location of the reference control's upper-left corner, in pixels.</param>
        /// <param name="host">The UI application's IPluginHost instance.</param>
        /// <param name="tags">An array of tags indicating the purpose of the data.</param>
        /// <param name="data">A list of object arrays whose type is respective to the requested type (in index) and whose elements are determined by the source plugin.</param>
        public static void XShow<T>(this T instance, T newInstance, Control control, System.Drawing.Point position, IPluginHost host, string[] tags, object[] data) where T : ContextMenuStrip
        {
            Type[] types = data.Select(a => a.GetType()).ToArray();
            Types.ContextMenuPath[] items = null;

            if (host != null && newInstance != null)
            {
                List<string> pluginItems = new List<string>();

                // Clear potential old items
                newInstance.Items.Clear();

                // Add in items from the original
                foreach (ToolStripMenuItem item in instance.Items)
                    newInstance.Items.Add(item.Clone());

                // Get new items from plugins
                foreach (IAddOn addOn in host.AddOns)
                {
                    items = addOn.OnXShow(tags, types);
                    if (items == null || items.Length == 0)
                        continue;

                    // Only add separator if one doesn't already exist
                    if (!(newInstance.Items[newInstance.Items.Count - 1] is ToolStripSeparator))
                        newInstance.Items.Add(new ToolStripSeparator());

                    // Loop through and add each item to the new context menu
                    foreach (Types.ContextMenuPath item in items)
                        newInstance.AddMenuStripItem(host, tags, data, item);
                }

                // Pop the menu and exit
                newInstance.Show(control, position);

                return;
            }

            // Show the default context menu
            instance.Show(control, position);
        }

        #region Add Menu Item from path

        private static void AddMenuStripItem<T>(this T menuStrip, IPluginHost host, string[] tags, object[] data, Types.ContextMenuPath citem) where T : ContextMenuStrip
        {
            if (citem.path == null || citem.callback == null)
                return;

            string[] pathParts = citem.path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int partIndex = 1;
            ToolStripMenuItem parent = null;

            if (pathParts.Length == 0)
                return;

            // Find first parent menu item
            foreach (var item in menuStrip.Items)
            {
                if (item is ToolStripMenuItem && ((ToolStripMenuItem)item).Text.ToLower() == pathParts[0].ToLower())
                {
                    parent = ((ToolStripMenuItem)item);
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

            parent.ToolTipText = citem.tooltip;
            parent.Click += (sender, e) =>
            {
                if (citem.requestedTypes == null || citem.requestedTypes.Length == 0)
                {
                    citem.callback.Invoke(host, tags, null);
                    return;
                }

                // Selectively grab the relevent data
                List<dynamic[]> selectData = new List<dynamic[]>();
                for (int y = 0; y < citem.requestedTypes.Length; y++)
                {
                    List<dynamic> tempData = new List<dynamic>();
                    for (int x = 0; x < data.Length; x++)
                    {
                        if (citem.requestedTypes[y] == null)
                            continue;
                        if (data[x] == null)
                            continue;
                        if (data[x].GetType() == citem.requestedTypes[y] || data[x].GetType().GetInterfaces().Contains(citem.requestedTypes[y]))
                        {
                            tempData.Add(data[x]);
                            break;
                        }
                    }

                    if (tempData.Count > 0)
                        selectData.Add(tempData.ToArray());
                }

                citem.callback.Invoke(host, tags, selectData);
            };
        }

        private static ToolStripMenuItem RecursiveAddToolStripItem(string[] pathParts, ref int index, ToolStripMenuItem parent)
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

        #region Clone Context Menu

        // The following code is from https://www.codeproject.com/articles/43472/a-pretty-good-menu-cloner
        // Licensed under The Common Development and Distribution License (CDDL) https://opensource.org/licenses/cddl1.php

        /// <summary>
        /// Clones the specified source tool strip menu item. 
        /// </summary>
        /// <param name="sourceToolStripMenuItem">The source tool strip menu item.</param>
        /// <returns>A cloned version of the toolstrip menu item</returns>
        public static ToolStripMenuItem Clone(this ToolStripMenuItem sourceToolStripMenuItem)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();

            var propInfoList = from p in typeof(ToolStripMenuItem).GetProperties()
                               let attributes = p.GetCustomAttributes(true)
                               let notBrowseable = (from a in attributes
                                                    where a.GetType() == typeof(BrowsableAttribute)
                                                    select !(a as BrowsableAttribute).Browsable).FirstOrDefault()
                               where !notBrowseable && p.CanRead && p.CanWrite && p.Name != "DropDown"
                               orderby p.Name
                               select p;

            // Copy over using reflections
            foreach (var propertyInfo in propInfoList)
            {
                object propertyInfoValue = propertyInfo.GetValue(sourceToolStripMenuItem, null);
                propertyInfo.SetValue(menuItem, propertyInfoValue, null);
            }

            // Create a new menu name
            menuItem.Name = sourceToolStripMenuItem.Name + "-" + menuNameCounter++;

            // Process any other properties
            if (sourceToolStripMenuItem.ImageIndex != -1)
            {
                menuItem.ImageIndex = sourceToolStripMenuItem.ImageIndex;
            }

            if (!string.IsNullOrEmpty(sourceToolStripMenuItem.ImageKey))
            {
                menuItem.ImageKey = sourceToolStripMenuItem.ImageKey;
            }

            // We need to make this visible 
            menuItem.Visible = true;

            // Recursively clone the drop down list
            foreach (var item in sourceToolStripMenuItem.DropDownItems)
            {
                ToolStripItem newItem;
                if (item is ToolStripMenuItem)
                {
                    newItem = ((ToolStripMenuItem)item).Clone();
                }
                else if (item is ToolStripSeparator)
                {
                    newItem = new ToolStripSeparator();
                }
                else
                {
                    throw new NotImplementedException("Menu item is not a ToolStripMenuItem or a ToolStripSeparatorr");
                }

                menuItem.DropDownItems.Add(newItem);
            }

            // The handler list starts empty because we created its parent via a new
            // So this is equivalen to a copy.
            menuItem.AddHandlers(sourceToolStripMenuItem);

            return menuItem;
        }

        /// <summary>
        /// Adds the handlers from the source component to the destination component
        /// </summary>
        /// <typeparam name="T">An IComponent type</typeparam>
        /// <param name="destinationComponent">The destination component.</param>
        /// <param name="sourceComponent">The source component.</param>
        public static void AddHandlers<T>(this T destinationComponent, T sourceComponent) where T : IComponent
        {
            // If there are other handlers, they will not be erased
            var destEventHandlerList = destinationComponent.GetEventHandlerList();
            var sourceEventHandlerList = sourceComponent.GetEventHandlerList();

            destEventHandlerList.AddHandlers(sourceEventHandlerList);
        }

        /// <summary>
        /// Gets the event handler list from a component
        /// </summary>
        /// <param name="component">The source component.</param>
        /// <returns>The EventHanderList or null if none</returns>
        public static EventHandlerList GetEventHandlerList(this IComponent component)
        {
            var eventsInfo = component.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            return (EventHandlerList)eventsInfo.GetValue(component, null);
        }

        #endregion

    }
}
