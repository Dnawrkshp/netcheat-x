using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NetCheatX.Core.UI;
using NetCheatX.Core;

namespace PCCommunicator
{
    public partial class Init : XForm
    {
        // Save our ICommunicator and our MemMan for later access
        private Communicator _communicator;
        private MemMan _manager;

        public Init(Communicator com, MemMan man)
        {
            InitializeComponent();

            // Define our private variables
            _communicator = com;
            _manager = man;

            // Refresh list of processes
            butRefresh_Click(null, null);
        }

        private void butAttach_Click(object sender, EventArgs e)
        {
            if (listProcess.SelectedIndex < 0)
                return;

            // Get Process ID and attach
            int pid = Convert.ToInt32(listProcess.SelectedItem.ToString().Split(' ')[0], 16);
            bool attach = _manager.Attach(pid);

            // Update ICommuncator's Ready property and, if atached, update lastPID
            if (attach)
                _communicator.lastPID = pid;
            _communicator.Ready = attach;
        }

        private void butRefresh_Click(object sender, EventArgs e)
        {
            
            listProcess.BeginUpdate();

            listProcess.Items.Clear();

            Process[] procs = Process.GetProcesses();
            procs = procs.OrderBy(a => a.Id).ToArray();
            foreach (Process proc in procs)
                listProcess.Items.Add(proc.Id.ToString("X8") + "  " + proc.ProcessName);

            listProcess.EndUpdate();
        }

        public override void SetTheme(Types.MetroTheme theme, Color background, Color foreground)
        {
            //base.SetTheme(theme, background, foreground);
        }
    }
}
