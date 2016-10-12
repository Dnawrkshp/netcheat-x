using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NetCheatX.Core;

namespace PCCommunicator
{
    public class Communicator : ICommunicator
    {
        private MemMan _manager = null;
        private bool _ready = false;

        // unique id of the Init form (used when NetCheat X is loading last saved UI layout)
        private string form_init_id = "CONNECT";

        public int lastPID = -1;
        

        public string Author { get; } = "Dan Gerendasy";

        public string Description { get; } = "PC Communicator Plugin for NetCheat X. Let's you scan any program on your PC with NetCheat X.";

        public string Name { get; } = "PC Communicator";

        public string Platform { get; } = "PC";

        public Types.Endian PlatformEndianess { get; } = (BitConverter.IsLittleEndian ? Types.Endian.LittleEndian : Types.Endian.BigEndian);

        public Types.BitArchitecture PlatformBitArchitecture { get; } = IntPtr.Size == 8 ? Types.BitArchitecture.bit64 : Types.BitArchitecture.bit32;

        public string Version { get; } = "1.0";

        public bool Ready
        {
            get { return _ready; }
            set
            {
                if (value != _ready)
                {
                    _ready = value;

                    if (ReadyChanged != null)
                        ReadyChanged.Invoke(this, Ready ? "Attached" : "Not Attached");
                }
            }
        }

        public event EventHandler<string> ReadyChanged;

        public bool GetBytes(ulong address, ref byte[] bytes)
        {
            if (!Ready || _manager == null)
                return false;

            return _manager.ReadMemory(address, ref bytes);
        }

        public bool SetBytes(ulong address, byte[] bytes)
        {
            if (!Ready || _manager == null)
                return false;

            return _manager.WriteMemory(address, bytes);
        }

        public Types.ProcessState GetProcessState()
        {
            if (!Ready || _manager == null || !_manager.Alive())
                return Types.ProcessState.Killed;

            if (_manager.isSuspended())
                return Types.ProcessState.Paused;

            return Types.ProcessState.Running;
        }

        public bool SetProcessState(Types.ProcessState state)
        {
            if (!Ready || _manager == null)
                return false;

            switch (state)
            {
                case Types.ProcessState.Running:
                    _manager.ContinueProcess();
                    break;
                case Types.ProcessState.Paused:
                    _manager.PauseProcess();
                    break;
                case Types.ProcessState.Killed:
                    _manager.KillProcess();
                    break;
            }

            return true;
        }

        public bool InitializeXForm(out NetCheatX.Core.UI.XForm xForm, string uniqueName)
        {
            // Go through each form ID and set xForm to initialized form
            // Return null if invalid uniqueName
            if (uniqueName == form_init_id)
            {
                xForm = new Init(this, _manager);
            }
            else
            {
                xForm = null;
                return false;
            }

            return true;
        }

        public bool Reconnect()
        {
            // Make sure our reconnect variables are valid
            if (_manager == null)
                return false;
            if (lastPID <= 0)
                return false;

            // Get current process state, attach process, return process to original state
            Types.ProcessState state = GetProcessState();
            bool attach = _manager.Attach(lastPID);
            if (attach)
                SetProcessState(state);
            return attach;
        }

        public void Initialize(IPluginHost host)
        {
            // Setup unique names for each form
            form_init_id = Name + " " + Version + " " + form_init_id;

            // Create MemMan instance
            _manager = new MemMan();

            // Register forms with UI
            host.RegisterWindow(this, "Attach Process", form_init_id, "Displays a list of processes to attach to.", AddInitForm);
        }

        public void Dispose(IPluginHost host)
        {
            form_init_id = null;
            _manager = null;
        }


        private bool AddInitForm(out NetCheatX.Core.UI.XForm form, IPluginHost host)
        {
            // Initialize our Attach Process form into form
            form = new Init(this, _manager);
            return true;
        }
    }
}
