using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchParamEditor
{
    public class typeSByte : ITypeEditor
    {
        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Edits data type SByte.";

        public string Name { get; } = "SByte Editor";

        public string Version { get; } = "1.0";

        // Data type
        public Type EditorType { get; } = typeof(sbyte);


        public void GetControl(out System.Windows.Forms.UserControl control)
        {
            // Return initialize editor control
            control = new UI.sbyteControl();
        }

        public void SetParam(System.Windows.Forms.UserControl control, byte[] param)
        {
            // Make sure control is valid
            if (control is UI.sbyteControl)
                (control as UI.sbyteControl).Value = (sbyte)param[0];
        }

        public bool ProcessParam(System.Windows.Forms.UserControl control, out byte[] param)
        {
            param = null;

            // Make sure control is of proper type
            if (control is UI.sbyteControl)
                return false;

            param = new byte[] { (byte)(control as UI.sbyteControl).Value };
            return true;
        }


        public void Initialize(IPluginHost host)
        {
            // Register this ITypeEditor
            host.RegisterTypeEditor(this);
        }

        public void Dispose(IPluginHost host)
        {
            
        }
    }
}
