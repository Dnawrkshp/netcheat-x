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
    public partial class intControl : UserControl
    {
        // Value of numericUpDown for public access
        public int Value
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public intControl()
        {
            InitializeComponent();

            numericUpDown1.Minimum = int.MinValue;
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Value = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Hexadecimal = checkBox1.Checked;
        }
    }
}
