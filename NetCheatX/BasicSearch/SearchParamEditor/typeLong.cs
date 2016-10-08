using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchParamEditor
{
    public class typeLong : ITypeEditor
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Edits data type Long.";

        public string Name { get; } = "Long Editor";

        public string Version { get; } = "1.0";

        // Data type
        public Type EditorType { get; } = typeof(long);


        public void GetControl(out System.Windows.Forms.UserControl control)
        {
            // Return initialize editor control
            control = new UI.longControl();
        }

        public void SetParam(System.Windows.Forms.UserControl control, byte[] param)
        {
            // Make sure control is valid
            if (control is UI.longControl)
                (control as UI.longControl).Value = _host.PlatformBitConverter.ToInt64(param, 0);
        }

        public bool ProcessParam(System.Windows.Forms.UserControl control, out byte[] param)
        {
            param = null;

            // Make sure control is of proper type
            if (control is UI.longControl)
                return false;

            param = _host.PlatformBitConverter.GetBytes((control as UI.longControl).Value);
            return true;
        }


        public void Initialize(IPluginHost host)
        {
            _host = host;

            // Register this ITypeEditor
            host.RegisterTypeEditor(this);
        }

        public void Dispose(IPluginHost host)
        {
            // Clean up
            _host = null;
        }
    }
}
