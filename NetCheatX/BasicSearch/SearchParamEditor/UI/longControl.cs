using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicSearch.SearchParamEditor.UI
{
    public partial class longControl : UserControl
    {
        // Value of numericUpDown for public access
        public long Value
        {
            get { return (long)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public longControl()
        {
            InitializeComponent();

            numericUpDown1.Minimum = long.MinValue;
            numericUpDown1.Maximum = long.MaxValue;
            numericUpDown1.Value = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Hexadecimal = checkBox1.Checked;
        }
    }
}
