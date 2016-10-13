using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode
{
    public class LegacyCodeEditor : ICodeEditor
    {
        private int _addFormCount = 0;
        private IPluginHost _host = null;

        // unique id of the Editor form (used when NetCheat X is loading last saved UI layout)
        private string form_editor_id = "LEGACYEDITOR";


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Code Editor for NetCheatPS3 code types.";

        public string Name { get; } = "Legacy Code Editor";

        public string[] SupportedPlatforms { get; } = null;

        public string Version { get; } = "1.0";


        public bool AddForm(out NetCheatX.Core.UI.XForm form, IPluginHost host)
        {
            form = new UI.Editor(host, Name + " " + Version + " [" + _addFormCount.ToString() + "]");

            // Keep track of number of LegacyCodeEditors added
            _addFormCount++;

            return true;
        }

        bool AddToCodes(IPluginHost host, ISearchType type, ISearchResult[] results, NetCheatX.Core.UI.XForm form)
        {
            string code;
            List<string> lines = new List<string>();
            UI.Editor editor = (UI.Editor)form;

            // Loop through and convert each result to a legacy code and add it
            foreach (ISearchResult result in results)
            {
                type.ResultToLegacyCode(out code, result);

                if (code != null && code.Length > 0)
                    lines.Add(code);
            }

            if (lines.Count > 0)
                editor.AddCode("NEW CODE", host.AuthorDefault, string.Join("\r\n", lines));

            return true;
        }


        // Execute all constant write codes
        public void ConstantExecute(IPluginHost host, NetCheatX.Core.UI.XForm xForm)
        {
            if (xForm == null || !(xForm is UI.Editor))
                return;

            UI.Editor editor = (UI.Editor)xForm;

            editor.Execute(true);
        }

        public bool InitializeXForm(out NetCheatX.Core.UI.XForm xForm, string uniqueName)
        {
            xForm = null;

            // Go through each form ID and set xForm to initialized form
            // Return null if invalid uniqueName
            if (uniqueName == form_editor_id)
            {
                xForm = new UI.Editor(_host, Name + " " + Version + " [" + _addFormCount.ToString() + "]");
                _addFormCount++;
                return true;
            }

            return false;
        }

        public void Initialize(IPluginHost host)
        {
            _host = host;

            // Setup unique names for each form
            form_editor_id = Name + " " + Version + " " + form_editor_id;

            host.RegisterWindow(this, this.Name, form_editor_id, Description, AddForm);
            host.RegisterAddCode(this, "Add code to code editor form.", AddToCodes);
        }

        public void Dispose(IPluginHost host)
        {
            _host = null;
            form_editor_id = null;
        }
    }
}
