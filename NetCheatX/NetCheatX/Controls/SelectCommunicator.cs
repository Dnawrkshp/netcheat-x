using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCheatX.UI.Controls
{
    public partial class SelectCommunicator : Form
    {
        private List<NetCheatX.Core.ICommunicator> _communicators = null;
        private Plugin.Host _host = null;

        public SelectCommunicator(Plugin.Host host, List<NetCheatX.Core.ICommunicator> communicators)
        {
            _host = host;
            _communicators = communicators;


            InitializeComponent();

            // List all ICommunicators
            foreach (NetCheatX.Core.ICommunicator com in communicators)
                lbComs.Items.Add(com.Name + " " + com.Version);

            if (lbComs.Items.Count > 0)
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
                "Platform Endianness: '" + _communicators[lbComs.SelectedIndex].PlatformEndianess.ToString() + "'\r\n" +
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

