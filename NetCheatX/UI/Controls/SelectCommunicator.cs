using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetCheatX.Core.Extensions;

namespace NetCheatX.UI.Controls
{
    public partial class SelectCommunicator : Form
    {
        private Core.Containers.PluginBaseContainer<NetCheatX.Core.Interfaces.ICommunicator> _communicators = null;
        private Plugin.PluginHost _host = null;

        public SelectCommunicator(Plugin.PluginHost host, Core.Containers.PluginBaseContainer<NetCheatX.Core.Interfaces.ICommunicator> communicators, string lastCommunicator)
        {
            _host = host;
            _communicators = communicators;


            InitializeComponent();

            this.Icon = Properties.Resources.ncx;

            // List all ICommunicators
            foreach (NetCheatX.Core.Interfaces.ICommunicator com in communicators)
            {
                lbComs.Items.Add(com.Name + " " + com.Version);
                if (lastCommunicator != null && com.ToBase64String() == lastCommunicator)
                    lbComs.SelectedIndex = lbComs.Items.Count - 1;
            }

            if (lbComs.Items.Count > 0 && lbComs.SelectedIndex < 0)
                lbComs.SelectedIndex = 0;
        }

        private void lbComs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbComs.SelectedIndex < 0)
            {
                lbDesc.Text = "";
                return;
            }

            lbDesc.Text = "Name: '" + _communicators[lbComs.SelectedIndex].Name + "'\r\n" +
                "Version: '" + _communicators[lbComs.SelectedIndex].Version + "'\r\n" +
                "Author(s): '" + _communicators[lbComs.SelectedIndex].Author + "'\r\n" +
                "Platform: '" + _communicators[lbComs.SelectedIndex].Platform + "'\r\n" +
                "Platform Endianness: '" + _communicators[lbComs.SelectedIndex].PlatformBitConverter.Endianness.ToString() + "'\r\n" +
                "\r\n\t" + _communicators[lbComs.SelectedIndex].Description;
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            if (lbComs.SelectedIndex < 0)
                return;

            _host.ActiveCommunicator = _communicators[lbComs.SelectedIndex];

            this.Close();
        }
    }
}

