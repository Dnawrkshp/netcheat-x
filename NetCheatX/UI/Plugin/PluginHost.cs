using NetCheatX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core.Bitlogic;
using NetCheatX.Core.Interfaces;
using NetCheatX.Core.Extensions;
using NetCheatX.Core.Containers;
using System.Drawing;

namespace NetCheatX.UI.Plugin
{
    public class PluginHost : IPluginHost
    {
        private bool _exitConstantWriteThread = false;
        private bool _constantWriteThreadHasReconnected = false;
        private System.Threading.Thread _constantWriteThread = null;

        #region Public Variables

        public WeifenLuo.WinFormsUI.Docking.DockPanel XFormDockPanel = null;
        public Settings.PlatformSetting PlatformSettings = null;
        public List<Core.UI.XForm> XForms = null;

        public List<Types.WindowItem> WindowItems = null;
        public List<Types.FunctionItem> FunctionItems = null;

        #endregion

        #region Events

        public event EventHandler<Types.WindowItem> WindowItemAdded;
        public event EventHandler<Types.FunctionItem> FunctionItemAdded;

        #endregion

        #region Inherited Properties

        private PluginBaseContainer<ICodeEditor> _codeEditors = null;
        private PluginBaseContainer<ICommunicator> _communicators = null;
        private PluginBaseContainer<IAddOn> _addOns = null;
        private PluginBaseContainer<ITypeEditor> _typeEditors = null;
        private PluginBaseContainer<ISearchMethod> _searchMethods = null;
        private PluginBaseContainer<ISearchType> _searchTypes = null;

        public PluginBaseContainer<ICodeEditor> CodeEditors { get { return _codeEditors; } }
        public PluginBaseContainer<ICommunicator> Communicators { get { return _communicators; } }
        public PluginBaseContainer<IAddOn> AddOns { get { return _addOns; } }
        public PluginBaseContainer<ITypeEditor> TypeEditors { get { return _typeEditors; } }
        public PluginBaseContainer<ISearchMethod> SearchMethods { get { return _searchMethods; } }
        public PluginBaseContainer<ISearchType> SearchTypes { get { return _searchTypes; } }

        private KeyValueContainer<string, string> _platformProperties = null;
        private KeyValueContainer<string, object> _tempProperties = null;

        public KeyValueContainer<string, string> PlatformProperties { get { return _platformProperties; } }
        public KeyValueContainer<string, object> TempProperties { get { return _tempProperties; } }

        private ICommunicator _activeCommunicator = null;
        public ICommunicator ActiveCommunicator
        {
            get { return _activeCommunicator; }
            set
            {
                // Remove ReadyChanged eventhandler on old communicator and add to new communicator
                if (_activeCommunicator != null)
                    _activeCommunicator.ReadyChanged -= ActiveCommunicator_ReadyChanged;
                _activeCommunicator = value;
                if (_activeCommunicator != null)
                    _activeCommunicator.ReadyChanged += ActiveCommunicator_ReadyChanged;
            }
        }

        #endregion

        public PluginHost()
        {
            _codeEditors = new PluginBaseContainer<ICodeEditor>();
            _communicators = new PluginBaseContainer<ICommunicator>();
            _addOns = new PluginBaseContainer<IAddOn>();
            _typeEditors = new PluginBaseContainer<ITypeEditor>();
            _searchMethods = new PluginBaseContainer<ISearchMethod>();
            _searchTypes = new PluginBaseContainer<ISearchType>();

            _platformProperties = new KeyValueContainer<string, string>();
            _tempProperties = new KeyValueContainer<string, object>();

            XForms = new List<Core.UI.XForm>();
            WindowItems = new List<Types.WindowItem>();
            FunctionItems = new List<Types.FunctionItem>();
        }

        #region Initialization and Cleanup

        // Run through each plugin and initialize
        public void InitializePlugins()
        {
            if (Program.pluginHandler.AvailablePlugins == null)
            {
                Program.logger.LogString("No plugins to initialize");
                return;
            }

            foreach (AvailablePlugin plugin in Program.pluginHandler.AvailablePlugins)
            {
                plugin.Instance.Initialize(this);
            }

            // Initialize constant write thread
            _constantWriteThreadHasReconnected = false;
            _exitConstantWriteThread = false;
            _constantWriteThread = new System.Threading.Thread(new System.Threading.ThreadStart(ConstantWriteThreadStart));
            _constantWriteThread.IsBackground = true;
            _constantWriteThread.Start();
        }

        public void Dispose()
        {
            Program.logger.LogString("Unloading Host");
            int x;

            if (Program.pluginHandler.AvailablePlugins == null)
                return;

            // Clean up constant write thread
            if (_constantWriteThread != null)
            {
                _exitConstantWriteThread = true;
                System.Threading.Thread.Sleep(100);
                _constantWriteThread.Abort();
                _constantWriteThread = null;
            }

            // Clean up settings
            if (PlatformSettings != null)
            {
                PlatformSettings.Close(XFormDockPanel);
                PlatformSettings.Dispose();
                PlatformSettings = null;
            }

            // Clean up properties
            if (_platformProperties != null && _platformProperties.Count > 0)
            {
                _platformProperties.SaveXML(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "Settings", this.ActiveCommunicator.Platform.ToLower() + ".pluginxml"));
                _platformProperties.Clear();
                _platformProperties = null;
            }

            // Clean up all interface lists
            for (x = 0; x < _codeEditors.Count; x++)
                try { _codeEditors[x].Dispose(this); } catch (Exception e) { Program.logger.LogException(e); }
            _codeEditors.Clear();

            for (x = 0; x < _communicators.Count; x++)
                try { _communicators[x].Dispose(this); } catch (Exception e) { Program.logger.LogException(e); }
            _communicators.Clear();

            for (x = 0; x < _addOns.Count; x++)
                try { _addOns[x].Dispose(this); } catch (Exception e) { Program.logger.LogException(e); }
            _addOns.Clear();

            for (x = 0; x < _typeEditors.Count; x++)
                try { _typeEditors[x].Dispose(this); } catch (Exception e) { Program.logger.LogException(e); }
            _typeEditors.Clear();

            for (x = 0; x < _searchMethods.Count; x++)
                try { _searchMethods[x].Dispose(this); } catch (Exception e) { Program.logger.LogException(e); }
            _searchMethods.Clear();

            for (x = 0; x < _searchTypes.Count; x++)
                try { _searchTypes[x].Dispose(this); } catch (Exception e) { Program.logger.LogException(e); }
            _searchTypes.Clear();

            // Clean up all plugins
            foreach (AvailablePlugin plugin in Program.pluginHandler.AvailablePlugins)
                try { plugin.Instance.Dispose(this); } catch (Exception e) { Program.logger.LogException(e); }

            // Clean up properties
            _tempProperties.Clear();

            // Clean up dockPanel
            XFormDockPanel = null;

            // Clean up Function and Window items
            FunctionItems.Clear();
            FunctionItems = null;
            WindowItems.Clear();
            WindowItems = null;

            // Clean up XForms
            if (XForms != null)
            {
                for (x = 0; x < XForms.Count; x++)
                {
                    XForms[x].Dispose();
                    XForms[x] = null;
                }

                XForms.Clear();
                XForms = null;
            }
        }

        #endregion


        #region Find Plugin

        // Find any IPluginBase by its Base64String
        public IPluginBase FindIPluginBase(string base64String)
        {
            foreach (IPluginBase item in _codeEditors)
                if (item.ToBase64String() == base64String)
                    return item;
            foreach (IPluginBase item in _addOns)
                if (item.ToBase64String() == base64String)
                    return item;
            foreach (IPluginBase item in _communicators)
                if (item.ToBase64String() == base64String)
                    return item;
            foreach (IPluginBase item in _searchMethods)
                if (item.ToBase64String() == base64String)
                    return item;
            foreach (IPluginBase item in _searchTypes)
                if (item.ToBase64String() == base64String)
                    return item;
            foreach (IPluginBase item in _typeEditors)
                if (item.ToBase64String() == base64String)
                    return item;

            return null;
        }

        #endregion

        #region Trigger Events

        public void TriggerWindowItemAdded(Types.WindowItem item)
        {
            if (WindowItemAdded != null)
                WindowItemAdded.Invoke(this, item);
        }

        public void TriggerFunctionItemAdded(Types.FunctionItem item)
        {
            if (FunctionItemAdded != null)
                FunctionItemAdded.Invoke(this, item);
        }

        #endregion

        #region Constant Write

        private void ConstantWriteThreadStart()
        {
            bool executed = false;
            int x = 0;

            while (!_exitConstantWriteThread)
            {
                executed = false;

                if (ActiveCommunicator == null || XForms == null || XForms.Count == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }

                // If this thread hasn't connected to platform/process and the UI form has then call Reconnect()
                if (!_constantWriteThreadHasReconnected && ActiveCommunicator.Ready)
                {
                    _constantWriteThreadHasReconnected = ActiveCommunicator.Reconnect();

                    if (!_constantWriteThreadHasReconnected)
                    {
                        System.Threading.Thread.Sleep(1000);
                        continue;
                    }
                }

                // Write all constant codes
                for (x = 0; x < XForms.Count; x++)
                {
                    try
                    {
                        // If XForm was registered by an ICodeEditor constant execute it
                        if (XForms[x] != null && XForms[x].ParentPlugin != null && XForms[x].ParentPlugin is ICodeEditor)
                        {
                            (XForms[x].ParentPlugin as ICodeEditor).ConstantExecute(this, XForms[x]);
                            executed = true;
                        }
                    }
                    catch (Exception e) { Program.logger.LogException(e); }
                }

                if (!executed)
                    System.Threading.Thread.Sleep(500);
            }
        }

        #endregion

        #region Event Handlers

        private void ActiveCommunicator_ReadyChanged(object sender, string e)
        {
            if (!ActiveCommunicator.Ready)
                _constantWriteThreadHasReconnected = false;
        }

        #endregion


        #region Log

        public void LogException(IPluginBase pluginBase, Exception e)
        {
            Program.logger.LogException(e, pluginBase.Name + " " + pluginBase.Version + " " + pluginBase.Author);
        }

        public void LogText(IPluginBase pluginBase, string text)
        {
            Program.logger.LogString(text, pluginBase.Name + " " + pluginBase.Version + " " + pluginBase.Author);
        }

        #endregion

        #region Register

        public bool RegisterWindow(IPluginBase pluginBase, string path, string uniqueName, string description, Core.Types.PluginWindowCallback callback)
        {
            Types.WindowItem item;

            // Add if the item doesn't already exist
            if (WindowItems.Where(x => x.UniqueName == uniqueName || (x.Path == path && x.ParentPlugin.GetType() == pluginBase.GetType())).FirstOrDefault() == null)
            {
                item = new Types.WindowItem() { Path = path, Callback = callback, Description = description, ParentPlugin = pluginBase, UniqueName = uniqueName };
                WindowItems.Add(item);
                TriggerWindowItemAdded(item);
                return true;
            }
            else
            {
                Program.logger.LogString("Attempt to register window with same path (" + path + ") or unique name (" + uniqueName + ") :: " + callback.Method.ToString(), pluginBase.Name + pluginBase.Version.ToString() + pluginBase.Author);
                return false;
            }
        }

        public bool RegisterFunction(IPluginBase pluginBase, string path, string description, Core.Types.PluginFunctionCallback callback)
        {
            Types.FunctionItem item;

            // Add if the item doesn't already exist
            if (FunctionItems.Where(x => x.Path == path && x.ParentPlugin.GetType() == pluginBase.GetType()).FirstOrDefault() == null)
            {
                item = new Types.FunctionItem() { Path = path, Callback = callback, Description = description, ParentPlugin = pluginBase, Icon = null };
                FunctionItems.Add(item);
                TriggerFunctionItemAdded(item);
                return true;
            }
            else
            {
                Program.logger.LogString("Attempt to register function with same path (" + path + ") :: " + callback.Method.ToString(), pluginBase.Name + pluginBase.Version.ToString() + pluginBase.Author);
                return false;
            }
        }

        public bool RegisterFunction(IPluginBase pluginBase, Image icon, string description, Core.Types.PluginFunctionCallback callback)
        {
            Types.FunctionItem item;

            // Add if the item doesn't already exist
            if (FunctionItems.Where(x => x.Icon == icon).FirstOrDefault() == null)
            {
                item = new Types.FunctionItem() { Path = null, Callback = callback, Description = description, ParentPlugin = pluginBase, Icon = icon };
                FunctionItems.Add(item);
                TriggerFunctionItemAdded(item);
                return true;
            }
            else
            {
                Program.logger.LogString("Attempt to register function with same icon (" + description + ") :: " + callback.Method.ToString(), pluginBase.Name + pluginBase.Version.ToString() + pluginBase.Author);
                return false;
            }
        }

        #endregion

        #region Private Functions

        // Determine if current platform is in list of supported platforms
        private bool SupportsPlatform(string[] supportedPlatforms)
        {
            if (supportedPlatforms == null)
                return true;

            string cmp = ActiveCommunicator.Platform.ToUpper();
            foreach (string platform in supportedPlatforms)
            {
                if (platform == null)
                    continue;

                if (cmp == platform.ToUpper())
                    return true;
            }

            return false;
        }

        #endregion

    }
}
