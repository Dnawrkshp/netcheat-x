using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCheatX.Core.UI
{
    /// <summary>
    /// XForm class for UI. All dockable UI forms must inherit this class.
    /// </summary>
    public partial class XForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        /// <summary>
        /// Unique name of specific type of XForm (designated by the plugin).
        /// </summary>
        public string UniqueName { get; set; }

        /// <summary>
        /// Parent plugin interface that added the XForm.
        /// </summary>
        public Interfaces.IPluginBase ParentPlugin { get; set; }

        /// <summary>
        /// Initializes new instance of <see cref="T:NetCheatX.Core.UI.XForm"/>
        /// </summary>
        public XForm()
        {
            InitializeComponent();
        }
    }
}
