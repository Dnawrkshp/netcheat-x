﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetCheatX.Core
{
    /// <summary>
    /// UI application plugin interface
    /// </summary>
    public interface IPluginHost
    {

        #region ICodeEditor

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

        #endregion

        #region ICommunicator

        /// <summary>
        /// Register a function under the Platform drop down
        /// </summary>
        /// <param name="com">ICommunicator calling function</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid path</returns>
        bool RegisterItem(ICommunicator com, string path, string description, Types.CommunicatorMenuItemCallback callback);

        /// <summary>
        /// Register a function on the toolbar menu
        /// </summary>
        /// <param name="com">ICommunicator calling function</param>
        /// <param name="icon">Image displayed</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid image</returns>
        bool RegisterItem(ICommunicator com, Image icon, string description, Types.CommunicatorMenuItemCallback callback);

        /// <summary>
        /// Add a new MDI child form to the UI.
        /// </summary>
        /// <param name="com">ICommunicator calling function</param>
        /// <param name="uniqueName">Unique name of form. Used when returning to a saved user layout</param>
        /// <param name="description">Information about window. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid uniqueName</returns>
        bool RegisterWindow(ICommunicator com, string uniqueName, string description, Types.CommunicatorMenuWindowCallback callback);

        #endregion

        #region ISearchMethod

        /// <summary>
        /// Register search method with Search UI
        /// </summary>
        /// <param name="searchMethod">Search method to add</param>
        /// <returns>True if succeeded. False if ISearchMethod with name already exists</returns>
        bool RegisterSearchMethod(ISearchMethod searchMethod);

        #endregion

        #region ISearchType

        /// <summary>
        /// Register search type with Search UI
        /// </summary>
        /// <param name="searchType">Search type to add</param>
        /// <returns>True if succeeded. False if ISearchType with name/version already exists</returns>
        bool RegisterSearchType(ISearchType searchType);

        #endregion

        #region ITypeEditor

        /// <summary>
        /// Register ITypeEditor with Search UI
        /// </summary>
        /// <param name="typeEditor">ITypeEditor to add</param>
        /// <returns>True if succeeded. False if ITypeEditor with name/version already exists</returns>
        bool RegisterTypeEditor(ITypeEditor typeEditor);

        #endregion

    }
}