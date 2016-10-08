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
    public partial class byteArrayControl : UserControl
    {
        // Value of numericUpDown for public access
        public string Value
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public byteArrayControl()
        {
            InitializeComponent();
        }
    }
}
