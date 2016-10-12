using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace NetCheatX.UI.Settings
{
    public class PlatformSetting
    {
        // Layout Document
        public struct Document
        {
            // Initial panel of display
            public DockPanelItem Panel;
            // All XForm FloatWindows (not docked)
            public List<XFormItem> FloatXForms;
        }

        // DockPanel control within document
        public struct DockPanelItem
        {
            // Size of panel
            public Size PanelSize;
            // Docking alignment of panel on parent
            public WeifenLuo.WinFormsUI.Docking.DockAlignment DockAlignment;
            // Child panels (if any)
            public List<DockPanelItem> PanelItems;
            // Child XForms (if any)
            public List<XFormItem> XFormItems;
        }

        // XForm control within document
        public struct XFormItem
        {
            // Unique name of XForm
            public string uniqueName;
            // ID of plugin interface that created XForm
            public string pluginID;
            // Dock Alignment of XForm (if docked)
            public WeifenLuo.WinFormsUI.Docking.DockAlignment DockAlignment;
            // Size of XForm (if floating)
            public Size XFormSize;
            // Location of XForm (if floating)
            public Point XFormLocation;
        }


        private Plugin.Host _host = null;


        public Document DocumentLayout;
        public Dictionary<Type, NetCheatX.Core.ITypeEditor> DefaultTypeEditor;
        public string Platform = null;
        public string Author = null;
        public Core.Types.MetroTheme Theme;

        public PlatformSetting(Plugin.Host host, string loadPath, string platformName)
        {
            if (host == null)
                return;

            // Set up variables
            _host = host;

            Platform = platformName;
            Theme = Core.Types.MetroTheme.Blue;

            Author = "";
            DocumentLayout = new Settings.PlatformSetting.Document();
            DocumentLayout.FloatXForms = new List<Settings.PlatformSetting.XFormItem>();

            DefaultTypeEditor = new Dictionary<Type, Core.ITypeEditor>();

            // Load existing Platform Settings
            if (loadPath != null && File.Exists(loadPath))
            {
                // Read all text
                string text = File.ReadAllText(loadPath);

                // Nothing to load
                if (text.Length == 0)
                    return;

                // Load file based on version
                if (text.StartsWith("1 "))
                    LoadSettings1(text);
            }
        }

        #region Load Settings Version 1 File Format

        // Definition
        private struct LS1Definition
        {
            public string type;
            public string[] values;
        }

        // Layout
        private struct LS1Layout
        {

        }

        // Version 1 PlatformSetting file format
        private void LoadSettings1(string text)
        {
            //[DEFINE] TYPE "VALUE0" "VALUE1" ... [/DEFINE]
            //[LAYOUT] ... [/LAYOUT]

            List<LS1Definition> defs;
            LoadDefinitions1(out defs, text);

            foreach (LS1Definition def in defs)
            {
                try
                {
                    switch (def.type)
                    {
                        case "PLATFORM": // Platform set by the Display class that owns this instance

                            break;
                        case "THEME": // Theme (only set on load)
                            Theme = (Core.Types.MetroTheme)Enum.Parse(typeof(Core.Types.MetroTheme), def.values[0]);
                            break;
                        case "AUTHOR": // Default author name
                            Author = def.values[0];
                            break;
                        case "ITYPEEDITOR": // Default ITypeEditor for specified type
                            // Load the type by its name
                            Type t = GetTypeFromAppDomain(def.values[0]);

                            // Get the ITypeEditor
                            NetCheatX.Core.ITypeEditor typeEditor = _host.FindITypeEditor(def.values[1]);
                            if (typeEditor != null && t != null && !DefaultTypeEditor.Keys.Contains(t))
                                DefaultTypeEditor.Add(t, typeEditor);
                            break;
                    }
                }
                catch { }
            }

        }
        
        // Load definitions from Version 1 PlatformSetting file format
        private void LoadDefinitions1(out List<LS1Definition> defs, string text)
        {
            defs = new List<LS1Definition>();

            int startIndex = 0, itemStartIndex = 0;
            string value, itemValue;
            LS1Definition item;

            while (EncapsulateTag(ref startIndex, out value, text, "[DEFINE]", "[/DEFINE]"))
            {
                itemStartIndex = 0;
                item = new LS1Definition() { values = new string[0] };


                // Load values into item
                while (EncapsulateTag(ref itemStartIndex, out itemValue, value, "\"", "\""))
                {
                    Array.Resize(ref item.values, item.values.Length + 1);
                    item.values[item.values.Length - 1] = itemValue;
                }

                // Load type
                item.type = value.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].ToUpper();

                // Add if valid definition type
                if (item.type.Length > 0)
                    defs.Add(item);
            }
        }
        

        #endregion

        private bool EncapsulateTag(ref int startIndex, out string value, string text, string startTag, string stopTag)
        {
            value = null;

            int startI = text.IndexOf(startTag, startIndex);
            if (startI < 0) // No tag found
                return false;

            int stopI = text.IndexOf(stopTag, startI + startTag.Length);
            if (stopI < 0) // Invalid tag
                return false;

            value = text.Substring(startI + startTag.Length, stopI - (startI + startTag.Length));
            startIndex = stopI + stopTag.Length;
            return true;
        }

        private Type GetTypeFromAppDomain(string fullTypeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = assembly.GetType(fullTypeName);
                if (t != null)
                    return t;
            }

            return null;
        }
    }
}
