using System;
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
        /// <summary>List of registered AddCode functions</summary>
        List<Types.AddCodeItem> AddCodeItems { get; }
        /// <summary>List of registered Windows</summary>
        List<Types.WindowItem> WindowItems { get; }
        /// <summary>List of registered menustrip/toolstrip items</summary>
        List<Types.MenuItem> MenuItems { get; }

        /// <summary>List of registered of ICodeEditors</summary>
        List<ICodeEditor> CodeEditors { get; }
        /// <summary>List of registered of ICommunicators</summary>
        List<ICommunicator> Communicators { get; }
        /// <summary>List of registered of IAddOns</summary>
        List<IAddOn> AddOns { get; }
        /// <summary>List of registered of ITypeEditors</summary>
        List<ITypeEditor> TypeEditors { get; }
        /// <summary>List of registered of ISearchMethods</summary>
        List<ISearchMethod> SearchMethods { get; }
        /// <summary>List of registered of ISearchTypes</summary>
        List<ISearchType> SearchTypes { get; }

        /// <summary>
        /// Get EndianBitConverter for converting with Platform endianness
        /// </summary>
        Bitlogic.EndianBitConverter PlatformBitConverter { get; }

        /// <summary>
        /// User entered Author name
        /// </summary>
        string AuthorDefault { get; }

        /// <summary>
        /// Reads memory at address into bytes via selected ICommunicator
        /// </summary>
        /// <param name="address">An unsigned 64-bit number</param>
        /// <param name="bytes">An array of bytes</param>
        /// <returns>True if succeeded. False if failed</returns>
        bool GetMemory(ulong address, ref byte[] bytes);

        /// <summary>
        /// Writes bytes into memory at address via selected ICommunicator
        /// </summary>
        /// <param name="address">An unsigned 64-bit number</param>
        /// <param name="bytes">An array of bytes</param>
        /// <returns>True if succeeded. False if failed</returns>
        bool SetMemory(ulong address, byte[] bytes);

        #region ICodeEditor

        /// <summary>
        /// Register code editor with UI
        /// </summary>
        /// <param name="codeEditor">ICodeEditor to register</param>
        /// <returns>True if succeeded. False if ICodeEditor with name/version already exists</returns>
        bool RegisterCodeEditor(ICodeEditor codeEditor);

        /// <summary>
        /// Register "Add To Codes" function in context menu of relevant UI
        /// Callback is called with code in NetCheat PS3 legacy format
        /// </summary>
        /// <param name="codeEditor">ICodeEditor calling function</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False</returns>
        bool RegisterAddCode(ICodeEditor codeEditor, string description, Types.CodeEditorAddCodeCallback callback);

        /// <summary>
        /// Register new form to the /View/Editor drop down menu
        /// </summary>
        /// <param name="codeEditor">ICodeEditor calling function</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="uniqueName">Unique name of form. Used when returning to a saved user layout</param>
        /// <param name="description">Information about window. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid uniqueName</returns>
        bool RegisterWindow(ICodeEditor codeEditor, string path, string uniqueName, string description, Types.AddXFormCallback callback);

        #endregion

        #region ICommunicator

        /// <summary>
        /// Register a function under the Platform drop down menu
        /// </summary>
        /// <param name="com">ICommunicator calling function</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid path</returns>
        bool RegisterItem(ICommunicator com, string path, string description, Types.PluginCallback callback);

        /// <summary>
        /// Register a function on the toolstrip menu
        /// </summary>
        /// <param name="com">ICommunicator calling function</param>
        /// <param name="icon">Image displayed</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid image</returns>
        bool RegisterItem(ICommunicator com, Image icon, string description, Types.PluginCallback callback);

        /// <summary>
        /// Register new form to the /View/Communicator drop down menu
        /// </summary>
        /// <param name="com">ICommunicator calling function</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="uniqueName">Unique name of form. Used when returning to a saved user layout</param>
        /// <param name="description">Information about window. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid uniqueName</returns>
        bool RegisterWindow(ICommunicator com, string path, string uniqueName, string description, Types.AddXFormCallback callback);

        /// <summary>
        /// Register class that inherits ICommunicator with UI application
        /// </summary>
        /// <param name="com">ICommunicator to register</param>
        /// <returns></returns>
        bool RegisterCommunicator(ICommunicator com);

        #endregion

        #region ISearchMethod

        /// <summary>
        /// Register search method with Search UI
        /// </summary>
        /// <param name="searchMethod">Search method to add</param>
        /// <returns>True if succeeded. False if ISearchMethod with name already exists</returns>
        bool RegisterSearchMethod(ISearchMethod searchMethod);

        /// <summary>
        /// Set the progress bar on the Search UI
        /// </summary>
        /// <param name="searchMethod">ISearchMethod calling</param>
        /// <param name="progress">Value from 0 to 100</param>
        /// <param name="description">Description of progress update</param>
        void SetProgress(ISearchMethod searchMethod, int progress, string description);

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

        #region IAddOn

        /// <summary>
        /// Register a function on the UI drop down menu
        /// </summary>
        /// <param name="addOn">IAddOn calling function</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid path</returns>
        bool RegisterItem(IAddOn addOn, string path, string description, Types.PluginCallback callback);

        /// <summary>
        /// Register a function on the UI toolstrip menu
        /// </summary>
        /// <param name="addOn">IAddOn calling function</param>
        /// <param name="icon">Image displayed</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid image</returns>
        bool RegisterItem(IAddOn addOn, Image icon, string description, Types.PluginCallback callback);

        /// <summary>
        /// Register new form to the /View/ drop down menu
        /// </summary>
        /// <param name="addOn">IAddOn calling function</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="uniqueName">Unique name of form. Used when returning to a saved user layout</param>
        /// <param name="description">Information about window. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid uniqueName</returns>
        bool RegisterWindow(IAddOn addOn, string path, string uniqueName, string description, Types.AddXFormCallback callback);

        #endregion

    }
}
