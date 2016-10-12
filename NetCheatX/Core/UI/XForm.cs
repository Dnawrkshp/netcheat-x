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
    /// XForm class for UI
    /// All dockable UI forms must inherit this class
    /// If you wish to influence the theming, override SetTheme()
    /// </summary>
    public partial class XForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        /// <summary>
        /// Unique name of specific type of XForm (designated by the plugin)
        /// </summary>
        public string UniqueName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public XForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the theme of the form and its controls
        /// </summary>
        /// <param name="theme">Theme</param>
        /// <param name="background">Theme default background color</param>
        /// <param name="foreground">Theme default foreground color</param>
        public virtual void SetTheme(Types.MetroTheme theme, Color background, Color foreground)
        {
            this.BackColor = background;
            this.ForeColor = foreground;
        }
    }
}
