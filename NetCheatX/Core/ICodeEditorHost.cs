using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// UI application code editor interface
    /// </summary>
    public interface ICodeEditorHost
    {
        /// <summary>
        /// Register "Add To Codes" function in context menu of relevant UI
        /// Callback is called with code in NetCheat PS3 legacy format
        /// </summary>
        /// <param name="codeEditor">ICommunicator calling function</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False</returns>
        bool RegisterAddCode(ICodeEditor codeEditor, string description, Types.CodeEditorAddCodeCallback callback);

        /// <summary>
        /// Add a new MDI child form to the UI.
        /// </summary>
        /// <param name="codeEditor">ICommunicator calling function</param>
        /// <param name="uniqueName">Unique name of form. Used when returning to a saved user layout</param>
        /// <param name="description">Information about window. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid uniqueName</returns>
        bool RegisterWindow(ICodeEditor codeEditor, string uniqueName, string description, Types.CommunicatorMenuWindowCallback callback);
    }
}
