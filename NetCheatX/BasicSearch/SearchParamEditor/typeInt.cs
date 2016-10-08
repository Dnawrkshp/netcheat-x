using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchParamEditor
{
    public class typeInt : ITypeEditor
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Edits data type Int.";

        public string Name { get; } = "Int Editor";

        public string Version { get; } = "1.0";

        // Data type
        public Type EditorType { get; } = typeof(int);


        public void GetControl(out System.Windows.Forms.UserControl control)
        {
            // Return initialize editor control
            control = new UI.intControl();
        }

        public void SetParam(System.Windows.Forms.UserControl control, byte[] param)
        {
            // Make sure control is valid
            if (control is UI.intControl)
                (control as UI.intControl).Value = _host.PlatformBitConverter.ToInt32(param, 0);
        }

        public bool ProcessParam(System.Windows.Forms.UserControl control, out byte[] param)
        {
            param = null;

            // Make sure control is of proper type
            if (control is UI.intControl)
                return false;

            param = _host.PlatformBitConverter.GetBytes((control as UI.intControl).Value);
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
