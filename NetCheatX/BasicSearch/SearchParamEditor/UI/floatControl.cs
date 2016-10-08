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
    public partial class floatControl : UserControl
    {
        // Value of numericUpDown for public access
        public float Value
        {
            get { return (float)numericUpDown1.Value; }
            set { numericUpDown1.Value = (decimal)value; }
        }

        public floatControl()
        {
            InitializeComponent();

            numericUpDown1.Minimum = decimal.MinValue;
            numericUpDown1.Maximum = decimal.MaxValue;
            numericUpDown1.Value = 0;
        }
    }

    // NumericUpDown without trailing decimal place zeros
    public class DecimalNumericUpDown : NumericUpDown
    {
        // Override this to format the displayed text
        protected override void UpdateEditText()
        {
            Text = Value.ToString("0." + new string('#', DecimalPlaces));
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            // Ensure value is as appears in text
            Value = Math.Round(Value, DecimalPlaces, MidpointRounding.AwayFromZero);

            base.OnValidating(e);
        }
    }
}
