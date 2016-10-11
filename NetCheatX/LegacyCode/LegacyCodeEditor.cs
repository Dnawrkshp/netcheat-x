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


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Code Editor for NetCheatPS3 code types.";

        public string Name { get; } = "Legacy Code Editor";

        public string[] SupportedPlatforms { get; } = null;

        public string Version { get; } = "1.0";


        public bool AddForm(out System.Windows.Forms.Form form, IPluginHost host)
        {
            form = new UI.Editor(host, Name + " " + Version + " [" + _addFormCount.ToString() + "]");

            // Keep track of number of LegacyCodeEditors added
            _addFormCount++;

            return true;
        }

        public void Initialize(IPluginHost host)
        {
            host.RegisterWindow(this, Name + " " + Version + " EDITOR", Description, AddForm);
            host.RegisterAddCode(this, "Add code to code editor form.", AddToCodes);
        }

        bool AddToCodes(IPluginHost host, ISearchType type, ISearchResult[] results, System.Windows.Forms.Form form)
        {
            string code;
            List<string> lines = new List<string>();
            UI.Editor editor = (UI.Editor)form;

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

        public void Dispose(IPluginHost host)
        {
            
        }
    }
}
