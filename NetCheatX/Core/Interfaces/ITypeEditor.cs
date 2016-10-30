using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// Type editor plugin interface.
    /// </summary>
    public interface ITypeEditor : IPluginBase
    {
        /// <summary>
        /// Data type the ITypeEditor can edit.
        /// </summary>
        Type EditorType { get; }

        /// <summary>
        /// Returns a new instance of ITypeEditor's UserControl.
        /// </summary>
        /// <param name="control">An uninitialized UserControl.</param>
        void GetControl(out System.Windows.Forms.UserControl control);

        /// <summary>
        /// Returns the unprocessed value contained in the UserControl
        /// </summary>
        /// <param name="control">Control to get value from.</param>
        /// <param name="value">An uninitialized object.</param>
        void GetUnprocessedParam(System.Windows.Forms.UserControl control, out object value);

        /// <summary>
        /// Sets the parameter value of the ITypeEditor's UserControl.
        /// </summary>
        /// <param name="control">Control to set value of.</param>
        /// <param name="value">Value to set.</param>
        void SetParam(System.Windows.Forms.UserControl control, byte[] value);

        /// <summary>
        /// Process the parameter.
        /// </summary>
        /// <param name="control">Control containing parameter to process.</param>
        /// <param name="param">An uninitialized array of bytes.</param>
        /// <returns>True if succeeded. False if failed.</returns>
        bool ProcessParam(System.Windows.Forms.UserControl control, out byte[] param);
    }
}
