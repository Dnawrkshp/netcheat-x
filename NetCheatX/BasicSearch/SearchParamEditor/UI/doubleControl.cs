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
    public partial class doubleControl : UserControl
    {
        // Value of numericUpDown for public access
        public double Value
        {
            get { return (double)numericUpDown1.Value; }
            set { numericUpDown1.Value = (decimal)value; }
        }

        public doubleControl()
        {
            InitializeComponent();

            numericUpDown1.Minimum = decimal.MinValue;
            numericUpDown1.Maximum = decimal.MaxValue;
            numericUpDown1.Value = 0;
        }
    }
}
