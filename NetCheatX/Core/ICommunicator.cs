﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Communicator plugin interface
    /// Bridges NetCheat X functionality with a platform
    /// </summary>
    public interface ICommunicator
    {
        /// <summary>
        /// Name of the ICommunicator
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the ICommunicator's purpose
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the ICommunicator
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the ICommunicator
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Name of platform (abbreviated, i.e. PC, PS3, XBOX, iOS)
        /// </summary>
        string Platform { get; }

        /// <summary>
        /// The endianess of the Platform
        /// </summary>
        Types.Endian PlatformEndianess { get; }

        /// <summary>
        /// If the target is ready for read/write interaction
        /// </summary>
        bool Ready { get; set; }

        /// <summary>
        /// Event raised when Ready property changes
        /// </summary>
        event EventHandler<string> ReadyChanged;

        /// <summary>
        /// Read bytes from memory of target process
        /// Returns read bytes into bytes array
        /// Returns false if failed
        /// </summary>
        bool GetBytes(ulong address, ref byte[] bytes);

        /// <summary>
        /// Write bytes to the memory of target process
        /// Returns false if failed
        /// </summary>
        bool SetBytes(ulong address, byte[] bytes);

        /// <summary>
        /// Set state of target process
        /// </summary>
        bool SetProcessState(Types.ProcessState state);

        /// <summary>
        /// Get state of target process
        /// </summary>
        Types.ProcessState GetProcessState();

        /// <summary>
        /// Initializes a new MDI child form identified by param uniqueName
        /// </summary>
        /// <param name="mdiForm">Uninitialized MDI child form</param>
        /// <param name="uniqueName">MDI child form identifier</param>
        /// <returns>False if failed</returns>
        bool InitializeMDIForm(out System.Windows.Forms.Form mdiForm, string uniqueName);

        /// <summary>
        /// Attempts to connect/attach to the target platform/program with the same parameters used in InitializeMDIForm()
        /// Called when a new thread needs access to communicator's read/write
        /// </summary>
        /// <returns>False if failed</returns>
        bool Reconnect();

        /// <summary>
        /// Called on initialization
        /// </summary>
        void Initialize(IPluginHost host);

        /// <summary>
        /// Called when disposed
        /// </summary>
        void Dispose(IPluginHost host);
    }
}
