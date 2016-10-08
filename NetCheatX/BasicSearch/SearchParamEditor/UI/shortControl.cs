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
    public partial class shortControl : UserControl
    {
        // Value of numericUpDown for public access
        public short Value
        {
            get { return (short)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public shortControl()
        {
            InitializeComponent();

            numericUpDown1.Minimum = short.MinValue;
            numericUpDown1.Maximum = short.MaxValue;
            numericUpDown1.Value = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Hexadecimal = checkBox1.Checked;
        }
    }
}
