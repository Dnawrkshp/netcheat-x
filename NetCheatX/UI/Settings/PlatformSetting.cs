using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Reflection;
using NetCheatX.Core.Extensions;

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


        private Plugin.PluginHost _host = null;


        public Document DocumentLayout;
        public Dictionary<Type, NetCheatX.Core.Interfaces.ITypeEditor> DefaultTypeEditor;
        public string Platform = null;
        public string Author = null;

        public PlatformSetting(Plugin.PluginHost host, string path, string platformName)
        {
            string iniPath = path + ".ini";
            string xmlPath = path + ".xml";

            if (host == null)
                return;

            // Set up variables
            _host = host;

            Platform = platformName;

            Author = "";
            DocumentLayout = new Settings.PlatformSetting.Document();
            DocumentLayout.FloatXForms = new List<Settings.PlatformSetting.XFormItem>();

            DefaultTypeEditor = new Dictionary<Type, Core.Interfaces.ITypeEditor>();

            // Load existing Platform Settings
            if (iniPath != null && File.Exists(iniPath))
            {
                // Read all text
                string text = File.ReadAllText(iniPath);

                // Nothing to load
                if (text.Length == 0)
                    return;

                // Load file based on version
                if (text.StartsWith("1 "))
                    LoadSettings1(text);
            }


            // Load layout
            if (xmlPath != null && File.Exists(xmlPath))
            {
                try { host.XFormDockPanel.LoadFromXml(xmlPath, FromPersistString); } catch (Exception e) { Program.logger.LogException(e); }
            }
        }

        // Close settings
        public void Close(WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel)
        {
            int index0, index1, x;
            string value;
            string[] lines;
            string tag = "PersistString";
            string output = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings", Platform.ToLower());

            // Save layout
            dockPanel.SaveAsXml(output + ".xml");

            //Update 'PersistString' to interface
            lines = File.ReadAllLines(output + ".xml");
            lines[0] = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"; // Fix utf-16 load error
            for (x = 0; x < lines.Length; x++)
            {
                index0 = lines[x].IndexOf(tag);
                if (index0 < 0)
                    continue;

                index1 = index0;
                index0 += tag.Length + 2;

                // Get existing value
                if (!EncapsulateTag(ref index1, out value, lines[x], "\"", "\""))
                    continue;

                // Get Type from PersistString value
                Type t = GetTypeFromAppDomain(value);
                if (t == null)
                    continue;

                foreach (Core.UI.XForm xform in _host.XForms)
                {
                    if (xform.GetType() == t)
                    {
                        // Remove existing value
                        lines[x] = lines[x].Remove(index0, value.Length);

                        // Generate new value
                        value = xform.ParentPlugin.ToBase64String() + " " + Convert.ToBase64String(Encoding.ASCII.GetBytes(xform.UniqueName));

                        // Insert new value
                        lines[x] = lines[x].Insert(index0, value);
                    }
                }
            }

            // Write new xml layout file
            File.WriteAllLines(output + ".xml", lines);

            // Write other settings
            StreamWriter ini = new StreamWriter(File.Create(output + ".ini"));

            ini.WriteLine("1 ");                                                        // Settings Version
            ini.WriteLine("[DEFINE] PLATFORM \"" + Platform + "\" [/DEFINE]");          // Platform
            ini.WriteLine("[DEFINE] AUTHOR \"" + Author + "\" [/DEFINE]");              // Default Author

            // Default Type Editors
            foreach (Type t in DefaultTypeEditor.Keys)
                ini.WriteLine("[DEFINE] ITYPEEDITOR \"" + t.ToString() + "\" \"" + DefaultTypeEditor[t].ToBase64String() + "\" [/DEFINE]");

            ini.Close();
        }

        public void Dispose()
        {
            DefaultTypeEditor.Clear();
            DefaultTypeEditor = null;

            Platform = null;
            Author = null;
        }

        #region Load Settings Version 1 File Format

        // Definition
        private struct LS1Definition
        {
            public string type;
            public string[] values;
        }

        // Version 1 PlatformSetting file format
        private void LoadSettings1(string text)
        {
            //[DEFINE] TYPE "VALUE0" "VALUE1" ... [/DEFINE]
            
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
                        case "AUTHOR": // Default author name
                            Author = def.values[0];
                            break;
                        case "ITYPEEDITOR": // Default ITypeEditor for specified type
                            // Load the type by its name
                            Type t = GetTypeFromAppDomain(def.values[0]);

                            // Get the ITypeEditor
                            Core.Interfaces.ITypeEditor typeEditor = (Core.Interfaces.ITypeEditor)_host.FindIPluginBase(def.values[1]);
                            if (typeEditor != null && t != null && !DefaultTypeEditor.Keys.Contains(t))
                                DefaultTypeEditor.Add(t, typeEditor);
                            break;
                    }
                }
                catch (Exception e) { Program.logger.LogException(e); }
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

        #region Private Load Functions

        private bool EncapsulateTag(ref int startIndex, out string value, string text, string startTag, string stopTag)
        {
            value = null;

            int startI = text.IndexOf(startTag, startIndex);
            if (startI < 0) // No tag found
                return false;

            int stopI = text.IndexOf(stopTag, startI + startTag.Length);
            if (stopI < 0) // Invalid tag
                return false;

            value = text.Substring(startI + startTag.Length, stopI - (startI + startTag.Length)).Trim();
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

        private Core.UI.XForm FromPersistString(string persistString)
        {
            Core.UI.XForm xForm = null;
            string[] words = persistString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] buffer = null; ;

            if (words.Length == 2) // IPluginBase XForm
            {
                buffer = Convert.FromBase64String(words[1]);
                words[1] = Encoding.ASCII.GetString(buffer);
                //words[1] = Encoding.UTF8.GetString(buffer);
                Core.Interfaces.IPluginBase ext = _host.FindIPluginBase(words[0]);
                if (ext == null)
                    return null;

                if (ext is Core.Interfaces.IAddOn)
                    (ext as Core.Interfaces.IAddOn).InitializeXForm(out xForm, words[1]);
                else if (ext is Core.Interfaces.ICodeEditor)
                    (ext as Core.Interfaces.ICodeEditor).InitializeXForm(out xForm, words[1]);
                else if (ext is Core.Interfaces.ICommunicator)
                    (ext as Core.Interfaces.ICommunicator).InitializeXForm(out xForm, words[1]);
                else
                    Program.logger.LogString("Error loading XForm from plugin " + ext.Name + ext.Version.ToString() + ext.Author);

                if (xForm != null)
                {
                    if (xForm.Icon == null)
                        xForm.Icon = Properties.Resources.ncx;
                    xForm.UniqueName = words[1];
                    xForm.ParentPlugin = ext;
                    _host.XForms.Add(xForm);
                }
            }
            else // UI Custom DockContent
            {

            }

            return xForm;
        }

        #endregion

    }
}
