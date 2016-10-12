using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Contains relevant data types and delegates
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// CPU Endianess
        /// </summary>
        public enum Endian
        {
            ///<summary>Little Endian</summary>
            LittleEndian,
            ///<summary>Big Endian</summary>
            BigEndian
        }

        /// <summary>
        /// CPU 32/64 bit architecture
        /// </summary>
        public enum BitArchitecture
        {
            ///<summary>32-bit architecture. Memory ranges from 0 to 0xFFFFFFFF</summary>
            bit32,
            ///<summary>64-bit architecture. Memory ranges from 0 to 0xFFFFFFFFFFFFFFFF</summary>
            bit64
        }

        /// <summary>
        /// Metro Themes supported by the UI
        /// </summary>
        public enum MetroTheme
        {
            ///<summary>Light theme</summary>
            Light,
            ///<summary>Blue theme</summary>
            Blue,
            ///<summary>Dark theme</summary>
            Dark
        }

        /// <summary>
        /// Process State
        /// </summary>
        public enum ProcessState
        {
            ///<summary>The process is actively running</summary>
            Running,
            ///<summary>The process is paused</summary>
            Paused,
            ///<summary>The process is terminated</summary>
            Killed
        }

        /// <summary>
        /// Search Mode
        /// </summary>
        public enum SearchMode
        {
            ///<summary>An ISearchMethod that can only be used for the first scan</summary>
            First,
            ///<summary>An ISearchMethod that can only be used after the first scan</summary>
            Next,
            ///<summary>An ISearchMethod that can be used for both the first and later scans</summary>
            Both
        }

        /// <summary>
        /// Search parameter
        /// </summary>
        public struct SearchParam
        {
            ///<summary>Name of parameter</summary>
            public string name;
            ///<summary>Type of parameter. Ignore if process is set true</summary>
            public Type type;
            ///<summary>If the parameter needs to be processed by an ISearchType first set this true. It wll be passed to your method as a byte array</summary>
            public bool process;
            /// <summary>Information on what the parameter is used for</summary>
            public string description;
        }

        /// <summary>
        /// Memory range
        /// </summary>
        public struct MemoryRange
        {
            ///<summary>Start of memory range</summary>
            public ulong start;
            ///<summary>Stop of memory range</summary>
            public ulong stop;
        }

        /// <summary>
        /// RegisterAddCode() list item
        /// </summary>
        public struct AddCodeItem
        {
            ///<summary>Source ICodeEditor</summary>
            public ICodeEditor codeEditor;
            ///<summary>Description of function</summary>
            public string description;
            ///<summary>Callback to add code</summary>
            public CodeEditorAddCodeCallback callback;
        }

        /// <summary>
        /// RegisterWindow() list item
        /// </summary>
        public struct WindowItem
        {
            ///<summary>Source ICodeEditor (if used)</summary>
            public ICodeEditor codeEditor;
            ///<summary>Source ICommunicator (if used)</summary>
            public ICommunicator communicator;
            ///<summary>Source IAddOn (if used)</summary>
            public IAddOn addOn;
            /// <summary>Path in menustrip</summary>
            public string path;
            ///<summary>Unique name of xform</summary>
            public string uniqueName;
            ///<summary>Description of xform</summary>
            public string description;
            ///<summary>Callback to initialize xform</summary>
            public AddXFormCallback callback;
        } 

        /// <summary>
        /// RegisterItem list item
        /// </summary>
        public struct MenuItem
        {
            ///<summary>Source IAddOn (if used)</summary>
            public IAddOn addOn;
            ///<summary>Source ICommunicator (if used)</summary>
            public ICommunicator communicator;
            ///<summary>Path in menustrip (if used)</summary>
            public string path;
            ///<summary>Description of menu item</summary>
            public string description;
            ///<summary>Image for toolstrip (if used)</summary>
            public System.Drawing.Image image;
            ///<summary>Callback when item is clicked</summary>
            public PluginCallback callback;
        }

        /// <summary>
        /// Callback function blueprint for plugins
        /// </summary>
        /// <param name="host">UI application IPluginHost instance</param>
        /// <returns></returns>
        public delegate bool PluginCallback(IPluginHost host);

        /// <summary>
        /// Callback function blueprint for adding new xform
        /// </summary>
        /// <param name="xForm">An uninitialized XForm</param>
        /// <param name="host">UI application IPluginHost instance</param>
        /// <returns></returns>
        public delegate bool AddXFormCallback(out UI.XForm xForm, IPluginHost host);

        /// <summary>
        /// Callback function blueprint for Code editor plugin AddToCodes context menu items
        /// </summary>
        /// <param name="host">UI application IPluginHost instance</param>
        /// <param name="type">ISearchType used</param>
        /// <param name="results">ISearchResult(s) to be added</param>
        /// <param name="xForm">Window to add code to</param>
        /// <returns></returns>
        public delegate bool CodeEditorAddCodeCallback(IPluginHost host, ISearchType type, ISearchResult[] results, NetCheatX.Core.UI.XForm xForm);

    }
}
