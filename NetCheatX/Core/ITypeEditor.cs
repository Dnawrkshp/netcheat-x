using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Type editor plugin interface
    /// </summary>
    public interface ITypeEditor
    {
        /// <summary>
        /// Name of the ITypeEditor
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the ITypeEditor's purpose
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the ITypeEditor
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the ITypeEditor
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Data type the ITypeEditor can edit
        /// </summary>
        Type EditorType { get; }

        /// <summary>
        /// Returns a new instance of ITypeEditor's UserControl
        /// </summary>
        /// <param name="control">An uninitialized UserControl</param>
        void GetControl(out System.Windows.Forms.UserControl control);

        /// <summary>
        /// Sets the parameter value of the ITypeEditor's UserControl
        /// </summary>
        /// <param name="control">Control to set value of</param>
        /// <param name="value">Value to set</param>
        void SetParam(System.Windows.Forms.UserControl control, byte[] value);

        /// <summary>
        /// Process the parameter
        /// </summary>
        /// <param name="control">Control containing parameter to process</param>
        /// <param name="param">An uninitialized arrry of bytes</param>
        /// <returns>True if succeeded. False if failed</returns>
        bool ProcessParam(System.Windows.Forms.UserControl control, out byte[] param);

        /// <summary>
        /// Called when the search UI is loaded
        /// </summary>
        void Initialize(IPluginHost host);

        /// <summary>
        /// Called when search UI is unloading
        /// </summary>
        void Dispose(IPluginHost host);
    }
}
