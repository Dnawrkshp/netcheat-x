using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// UI application plugin interface
    /// </summary>
    public interface IPluginHost
    {
        /// <summary>List of registered of ICommunicators</summary>
        Containers.PluginBaseContainer<ICommunicator> Communicators { get; }
        /// <summary>List of registered of IAddOns</summary>
        Containers.PluginBaseContainer<IAddOn> AddOns { get; }
        /// <summary>List of registered of ITypeEditors</summary>
        Containers.PluginBaseContainer<ITypeEditor> TypeEditors { get; }
        /// <summary>List of registered of ISearchMethods</summary>
        Containers.PluginBaseContainer<ISearchMethod> SearchMethods { get; }
        /// <summary>List of registered of ISearchTypes</summary>
        Containers.PluginBaseContainer<ISearchType> SearchTypes { get; }

        /// <summary>
        /// Contains a collection of saved properties that are specific to the user-specified platform.
        /// </summary>
        Containers.KeyValueContainer<string, string> PlatformProperties { get; }

        /// <summary>
        /// Contains a collection of properties that are never saved.
        /// </summary>
        Containers.KeyValueContainer<string, object> TempProperties { get; }

        /// <summary>
        /// Gets the current Communicator
        /// </summary>
        ICommunicator ActiveCommunicator { get; }

        /// <summary>
        /// Reserved for memory writing, this event is triggered frequently on a background thread.
        /// </summary>
        event Types.PluginFunctionCallback ConstantWrite;

        #region Log

        /// <summary>
        /// Write Exception to the log file
        /// </summary>
        /// <param name="pluginBase">Plugin making call</param>
        /// <param name="e">Exception to write to log file</param>
        void LogException(IPluginBase pluginBase, Exception e);

        /// <summary>
        /// Write text to the log file
        /// </summary>
        /// <param name="pluginBase">Plugin making call</param>
        /// <param name="text">Text to write to log file</param>
        void LogText(IPluginBase pluginBase, string text);

        #endregion

        #region Register

        /// <summary>
        /// Register new form to the /View/ drop down menu
        /// </summary>
        /// <param name="pluginBase">An instance of IPluginBase</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="uniqueName">Unique name of form. Used when returning to a saved user layout</param>
        /// <param name="description">Information about window. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid uniqueName</returns>
        bool RegisterWindow(IPluginBase pluginBase, string path, string uniqueName, string description, Types.PluginWindowCallback callback);

        /// <summary>
        /// Register a function to the drop down menu
        /// </summary>
        /// <param name="pluginBase">An instance of IPluginBase</param>
        /// <param name="path">Drop down path. Ex: "Extra/Temperature"</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid path</returns>
        bool RegisterFunction(IPluginBase pluginBase, string path, string description, Types.PluginFunctionCallback callback);

        /// <summary>
        /// Register a function to the toolstrip menu
        /// </summary>
        /// <param name="pluginBase">An instance of IPluginBase</param>
        /// <param name="icon">Image displayed</param>
        /// <param name="description">Information about function. Displayed in a ToolTip</param>
        /// <param name="callback">Function called when drop down item clicked</param>
        /// <returns>True if succeeded. False if invalid image</returns>
        bool RegisterFunction(IPluginBase pluginBase, Image icon, string description, Types.PluginFunctionCallback callback);

        #endregion

    }
}
