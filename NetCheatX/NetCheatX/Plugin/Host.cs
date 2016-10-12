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


        public ICommunicator ActiveCommunicator = null;
        public Settings.PlatformSetting PlatformSettings = null;

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
        }

        public void Dispose()
        {
            int x;

            if (Program.pluginHandler.AvailablePlugins == null)
                return;

            // Clean up all interface lists
            for (x = 0; x < _codeEditors.Count; x++)
                _codeEditors[x].Dispose(this);
            _codeEditors.Clear();

            for (x = 0; x < _communicators.Count; x++)
                _communicators[x].Dispose(this);
            _communicators.Clear();

            for (x = 0; x < _addOns.Count; x++)
                _addOns[x].Dispose(this);
            _addOns.Clear();

            for (x = 0; x < _typeEditors.Count; x++)
                _typeEditors[x].Dispose(this);
            _typeEditors.Clear();

            for (x = 0; x < _searchMethods.Count; x++)
                _searchMethods[x].Dispose(this);
            _searchMethods.Clear();

            for (x = 0; x < _searchTypes.Count; x++)
                _searchTypes[x].Dispose(this);
            _searchTypes.Clear();

            // Clean up all plugins
            foreach (Core.IPlugin plugin in Program.pluginHandler.AvailablePlugins)
                plugin.Dispose(this);
        }

        #region Find Plugin

        // Find ITypeEditor by it's nameVersionAuthor descriptors
        public ITypeEditor FindITypeEditor(string nameVersionAuthor)
        {
            string name;

            foreach (ITypeEditor typeEditor in _typeEditors)
            {
                name = typeEditor.Name + " " + typeEditor.Version + " " + typeEditor.Author;
                if (name == nameVersionAuthor)
                {
                    name = null;
                    return typeEditor;
                }
            }

            return null;
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

            Types.WindowItem item = new Types.WindowItem() { codeEditor = codeEditor, path = path, uniqueName = uniqueName, description = description, callback = callback };

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
            Types.MenuItem item = new Types.MenuItem() { communicator = com, image = icon, description = description, callback = callback };

            if (_menuItems.Contains(item))
                return false;

            _menuItems.Add(item);

            if (MenuItemAdded != null)
                MenuItemAdded.Invoke(this, item);
            return true;
        }

        public bool RegisterItem(ICommunicator com, string path, string description, Types.PluginCallback callback)
        {
            Types.MenuItem item = new Types.MenuItem() { communicator = com, path = path, description = description, callback = callback };

            if (_menuItems.Contains(item))
                return false;

            _menuItems.Add(item);

            if (MenuItemAdded != null)
                MenuItemAdded.Invoke(this, item);
            return true;
        }

        public bool RegisterWindow(ICommunicator com, string path, string uniqueName, string description, Types.AddXFormCallback callback)
        {
            Types.WindowItem item = new Types.WindowItem() { communicator = com, path = path, uniqueName = uniqueName, description = description, callback = callback };

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

            Types.MenuItem item = new Types.MenuItem() { addOn = addOn, image = icon, description = description, callback = callback };

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

            Types.MenuItem item = new Types.MenuItem() { addOn = addOn, path = path, description = description, callback = callback };

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

            Types.WindowItem item = new Types.WindowItem() { addOn = addOn, uniqueName = uniqueName, description = description, callback = callback };

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
