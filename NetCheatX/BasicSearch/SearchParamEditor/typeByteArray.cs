using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch.SearchParamEditor
{
    public class typeByteArray : ITypeEditor
    {
        private IPluginHost _host = null;


        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "Edits data type Byte[].";

        public string Name { get; } = "Byte[] Editor";

        public string Version { get; } = "1.0";

        // Data type
        public Type EditorType { get; } = typeof(byte[]);


        public void GetControl(out System.Windows.Forms.UserControl control)
        {
            // Return initialize editor control
            control = new UI.byteArrayControl();
        }

        public void SetParam(System.Windows.Forms.UserControl control, byte[] param)
        {
            // Make sure control is valid
            if (control is UI.byteArrayControl)
                (control as UI.byteArrayControl).Value = _host.PlatformBitConverter.ToString(param).Replace("-", "");
        }

        public bool ProcessParam(System.Windows.Forms.UserControl control, out byte[] param)
        {
            param = null;

            // Make sure control is of proper type
            if (!(control is UI.byteArrayControl))
                return false;

            param = (BitConverter.IsLittleEndian ? Globals.LittleEndianBitConverter : Globals.BigEndianBitConverter).GetBytes((control as UI.byteArrayControl).Value);
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
