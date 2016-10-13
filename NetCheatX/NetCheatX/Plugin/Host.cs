using NetCheatX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core.Bitlogic;
using System.Drawing;

namespace NetCheatX.UI.Plugin
{
    public class Host : IPluginHost
    {
        private List<Types.AddCodeItem> _addCodeItems = null;
        private List<Types.WindowItem> _windowItems = null;
        private List<Types.MenuItem> _menuItems = null;

        private List<ICodeEditor> _codeEditors = null;
        private List<ICommunicator> _communicators = null;
        private List<IAddOn> _addOns = null;
        private List<ITypeEditor> _typeEditors = null;
        private List<ISearchMethod> _searchMethods = null;
        private List<ISearchType> _searchTypes = null;

        private bool _exitConstantWriteThread = false;
        private bool _constantWriteThreadHasReconnected = false;
        private System.Threading.Thread _constantWriteThread = null;

        private ICommunicator _activeCommunicator = null;

        public WeifenLuo.WinFormsUI.Docking.DockPanel XFormDockPanel = null;
        public Settings.PlatformSetting PlatformSettings = null;
        public List<Core.UI.XForm> XForms = null;

        #region Property

        public List<Types.AddCodeItem> AddCodeItems { get { return _addCodeItems; } }
        public List<Types.WindowItem> WindowItems { get { return _windowItems; } }
        public List<Types.MenuItem> MenuItems { get { return _menuItems; } }

        public List<ICodeEditor> CodeEditors { get { return _codeEditors; } }
        public List<ICommunicator> Communicators { get { return _communicators; } }
        public List<IAddOn> AddOns { get { return _addOns; } }
        public List<ITypeEditor> TypeEditors { get { return _typeEditors; } }
        public List<ISearchMethod> SearchMethods { get { return _searchMethods; } }
        public List<ISearchType> SearchTypes { get { return _searchTypes; } }

        public string AuthorDefault { get { return (PlatformSettings == null ? "" : (PlatformSettings.Author == null ? "" : PlatformSettings.Author)); } }

        public EndianBitConverter PlatformBitConverter
        {
            get
            {
                if (ActiveCommunicator == null)
                    return Globals.DefaultBitConverter;
                return ActiveCommunicator.PlatformEndianess == Types.Endian.LittleEndian ? Globals.LittleEndianBitConverter : Globals.BigEndianBitConverter;
            }
        }

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

        #region Events

        public event EventHandler<Types.AddCodeItem> CodeItemAdded;
        public event EventHandler<Types.WindowItem> WindowItemAdded;
        public event EventHandler<Types.MenuItem> MenuItemAdded;

        #endregion

        public Host()
        {
            _addCodeItems = new List<Types.AddCodeItem>();
            _windowItems = new List<Types.WindowItem>();
            _menuItems = new List<Types.MenuItem>();

            _codeEditors = new List<ICodeEditor>();
            _communicators = new List<ICommunicator>();
            _addOns = new List<IAddOn>();
            _typeEditors = new List<ITypeEditor>();
            _searchMethods = new List<ISearchMethod>();
            _searchTypes = new List<ISearchType>();

            XForms = new List<Core.UI.XForm>();
        }

        // Run through each plugin and initialize
        public void InitializePlugins()
        {
            if (Program.pluginHandler.AvailablePlugins == null)
                return;

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

            // Clean up register items
            if (_addCodeItems != null)
            {
                _addCodeItems.Clear();
                _addCodeItems = null;
            }

            if (_windowItems != null)
            {
                _windowItems.Clear();
                _windowItems = null;
            }

            if (_menuItems != null)
            {
                _menuItems.Clear();
                _menuItems = null;
            }

            // Clean up dockPanel
            XFormDockPanel = null;

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

        #region Find Plugin

        // Find ITypeEditor by it's nameVersionAuthor descriptors
        public ITypeEditor FindITypeEditor(string nameVersionAuthor)
        {
            foreach (ITypeEditor typeEditor in _typeEditors)
            {
                if (IPluginExtensionToString(typeEditor) == nameVersionAuthor)
                    return typeEditor;
            }

            return null;
        }

        // Find any IPluginExtension by it's nameVersionAuthor descriptors
        public IPluginExtension FindIPluginExtension(string nameVersionAuthor)
        {
            foreach (IPluginExtension item in _codeEditors)
                if (IPluginExtensionToString(item) == nameVersionAuthor)
                    return item;
            foreach (IPluginExtension item in _addOns)
                if (IPluginExtensionToString(item) == nameVersionAuthor)
                    return item;
            foreach (IPluginExtension item in _communicators)
                if (IPluginExtensionToString(item) == nameVersionAuthor)
                    return item;
            foreach (IPluginExtension item in _searchMethods)
                if (IPluginExtensionToString(item) == nameVersionAuthor)
                    return item;
            foreach (IPluginExtension item in _searchTypes)
                if (IPluginExtensionToString(item) == nameVersionAuthor)
                    return item;
            foreach (IPluginExtension item in _typeEditors)
                if (IPluginExtensionToString(item) == nameVersionAuthor)
                    return item;

            return null;
        }

        // Process IPluginExtension into NameVersionAuthor string
        public string IPluginExtensionToString(IPluginExtension pluginExtension)
        {
            return pluginExtension.Name.Replace("\"", "%22").Replace(" ", "%20").Replace("\t", "%09").Replace("\r", "%20").Replace("\n", "%20") +
                pluginExtension.Version.Replace("\"", "%22").Replace(" ", "%20").Replace("\t", "%09").Replace("\r", "%20").Replace("\n", "%20") +
                pluginExtension.Author.Replace("\"", "%22").Replace(" ", "%20").Replace("\t", "%09").Replace("\r", "%20").Replace("\n", "%20");

        }

        #endregion

        #region Trigger Events

        public void TriggerCodeItemAdded(Types.AddCodeItem item)
        {
            if (CodeItemAdded != null)
                CodeItemAdded.Invoke(this, item);
        }

        public void TriggerWindowItemAdded(Types.WindowItem item)
        {
            if (WindowItemAdded != null)
                WindowItemAdded.Invoke(this, item);
        }

        public void TriggerMenuItemAdded(Types.MenuItem item)
        {
            if (MenuItemAdded != null)
                MenuItemAdded.Invoke(this, item);
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


        #region Read/Write

        public bool GetMemory(ulong address, ref byte[] bytes)
        {
            if (ActiveCommunicator == null)
                return false;

            return ActiveCommunicator.GetBytes(address, ref bytes);
        }

        public bool SetMemory(ulong address, byte[] bytes)
        {
            if (ActiveCommunicator == null)
                return false;

            return ActiveCommunicator.SetBytes(address, bytes);
        }

        #endregion

        #region ICodeEditor

        public bool RegisterAddCode(ICodeEditor codeEditor, string description, Types.CodeEditorAddCodeCallback callback)
        {
            if (!SupportsPlatform(codeEditor.SupportedPlatforms))
                return false;

            Types.AddCodeItem item = new Types.AddCodeItem() { callback = callback, codeEditor = codeEditor, description = description };

            if (_addCodeItems.Contains(item))
                return false;

            _addCodeItems.Add(item);

            if (CodeItemAdded != null)
                CodeItemAdded.Invoke(this, item);
            return true;
        }

        public bool RegisterCodeEditor(ICodeEditor codeEditor)
        {
            if (!SupportsPlatform(codeEditor.SupportedPlatforms))
                return false;

            if (_codeEditors.Contains(codeEditor))
                return false;

            _codeEditors.Add(codeEditor);

            codeEditor.Initialize(this);
            return true;
        }

        public bool RegisterWindow(ICodeEditor codeEditor, string path, string uniqueName, string description, Types.AddXFormCallback callback)
        {
            if (!SupportsPlatform(codeEditor.SupportedPlatforms))
                return false;

            Types.WindowItem item = new Types.WindowItem() { pluginExtension = codeEditor, path = path, uniqueName = uniqueName, description = description, callback = callback };

            if (_windowItems.Contains(item))
                return false;

            _windowItems.Add(item);

            if (WindowItemAdded != null)
                WindowItemAdded.Invoke(this, item);
            return true;
        }

        #endregion

        #region ICommunicator

        public bool RegisterCommunicator(ICommunicator com)
        {
            if (_communicators.Contains(com))
                return false;

            _communicators.Add(com);

            com.Initialize(this);
            return true;
        }

        public bool RegisterItem(ICommunicator com, Image icon, string description, Types.PluginCallback callback)
        {
            Types.MenuItem item = new Types.MenuItem() { pluginExtension = com, image = icon, description = description, callback = callback };

            if (_menuItems.Contains(item))
                return false;

            _menuItems.Add(item);

            if (MenuItemAdded != null)
                MenuItemAdded.Invoke(this, item);
            return true;
        }

        public bool RegisterItem(ICommunicator com, string path, string description, Types.PluginCallback callback)
        {
            Types.MenuItem item = new Types.MenuItem() { pluginExtension = com, path = path, description = description, callback = callback };

            if (_menuItems.Contains(item))
                return false;

            _menuItems.Add(item);

            if (MenuItemAdded != null)
                MenuItemAdded.Invoke(this, item);
            return true;
        }

        public bool RegisterWindow(ICommunicator com, string path, string uniqueName, string description, Types.AddXFormCallback callback)
        {
            Types.WindowItem item = new Types.WindowItem() { pluginExtension = com, path = path, uniqueName = uniqueName, description = description, callback = callback };

            if (_windowItems.Contains(item))
                return false;

            _windowItems.Add(item);

            if (WindowItemAdded != null)
                WindowItemAdded.Invoke(this, item);
            return true;
        }

        #endregion

        #region IAddOn

        public bool RegisterItem(IAddOn addOn, Image icon, string description, Types.PluginCallback callback)
        {
            if (!SupportsPlatform(addOn.SupportedPlatforms))
                return false;

            Types.MenuItem item = new Types.MenuItem() { pluginExtension = addOn, image = icon, description = description, callback = callback };

            if (_menuItems.Contains(item))
                return false;

            _menuItems.Add(item);

            if (MenuItemAdded != null)
                MenuItemAdded.Invoke(this, item);
            return true;
        }

        public bool RegisterItem(IAddOn addOn, string path, string description, Types.PluginCallback callback)
        {
            if (!SupportsPlatform(addOn.SupportedPlatforms))
                return false;

            Types.MenuItem item = new Types.MenuItem() { pluginExtension = addOn, path = path, description = description, callback = callback };

            if (_menuItems.Contains(item))
                return false;

            _menuItems.Add(item);

            if (MenuItemAdded != null)
                MenuItemAdded.Invoke(this, item);
            return true;
        }

        public bool RegisterWindow(IAddOn addOn, string path, string uniqueName, string description, Types.AddXFormCallback callback)
        {
            if (!SupportsPlatform(addOn.SupportedPlatforms))
                return false;

            Types.WindowItem item = new Types.WindowItem() { pluginExtension = addOn, uniqueName = uniqueName, description = description, callback = callback };

            if (_windowItems.Contains(item))
                return false;

            _windowItems.Add(item);

            if (WindowItemAdded != null)
                WindowItemAdded.Invoke(this, item);
            return true;
        }

        #endregion

        #region ISearchMethod

        public bool RegisterSearchMethod(ISearchMethod searchMethod)
        {
            if (!SupportsPlatform(searchMethod.SupportedPlatforms))
                return false;

            if (_searchMethods.Contains(searchMethod))
                return false;

            _searchMethods.Add(searchMethod);

            searchMethod.Initialize(this);
            return true;
        }

        public void SetProgress(ISearchMethod searchMethod, int progress, string description)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISearchType

        public bool RegisterSearchType(ISearchType searchType)
        {
            if (!SupportsPlatform(searchType.SupportedPlatforms))
                return false;

            if (_searchTypes.Contains(searchType))
                return false;

            _searchTypes.Add(searchType);

            searchType.Initialize(this);
            return true;
        }

        #endregion

        #region ITypeEditor

        public bool RegisterTypeEditor(ITypeEditor typeEditor)
        {
            if (_typeEditors.Contains(typeEditor))
                return false;

            _typeEditors.Add(typeEditor);

            typeEditor.Initialize(this);
            return true;
        }

        #endregion


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

    }
}
