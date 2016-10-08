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
    public partial class ushortControl : UserControl
    {
        // Value of numericUpDown for public access
        public ushort Value
        {
            get { return (ushort)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public ushortControl()
        {
            InitializeComponent();

            numericUpDown1.Minimum = ushort.MinValue;
            numericUpDown1.Maximum = ushort.MaxValue;
            numericUpDown1.Value = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Hexadecimal = checkBox1.Checked;
        }
    }
}
