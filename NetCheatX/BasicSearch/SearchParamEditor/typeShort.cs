using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchParamEditor
{
    public class typeShort : ITypeEditor
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Edits data type Short.";

        public string Name { get; } = "Short Editor";

        public string Version { get; } = "1.0";

        // Data type
        public Type EditorType { get; } = typeof(short);


        public void GetControl(out System.Windows.Forms.UserControl control)
        {
            // Return initialize editor control
            control = new UI.shortControl();
        }

        public void SetParam(System.Windows.Forms.UserControl control, byte[] param)
        {
            // Make sure control is valid
            if (control is UI.shortControl)
                (control as UI.shortControl).Value = _host.PlatformBitConverter.ToInt16(param, 0);
        }

        public bool ProcessParam(System.Windows.Forms.UserControl control, out byte[] param)
        {
            param = null;

            // Make sure control is of proper type
            if (control is UI.shortControl)
                return false;

            param = _host.PlatformBitConverter.GetBytes((control as UI.shortControl).Value);
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
