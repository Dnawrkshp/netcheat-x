using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegacyCode.UI
{
    public partial class CodeEditor : UserControl
    {
        private bool _isUserInput = true;

        
        public event EventHandler<string> CodeChanged;
        public event EventHandler<string> NameChanged;
        public event EventHandler<string> AuthorChanged;
        public event EventHandler<bool> ConstantChanged;
        public event EventHandler WriteActivated;

        public bool Constant
        {
            get { return cbConstant.Checked; }
            set
            {
                _isUserInput = false;
                cbConstant.Checked = value;
                _isUserInput = true;
            }
        }

        public string CodeName
        {
            get { return tbName.Text; }
            set
            {
                _isUserInput = false;
                tbName.Text = value;
                _isUserInput = true;
            }
        }

        public string CodeAuthor
        {
            get { return tbAuthor.Text; }
            set
            {
                _isUserInput = false;
                tbAuthor.Text = value;
                _isUserInput = true;
            }
        }

        public string CodeText
        {
            get { return tbCode.Text; }
            set
            {
                _isUserInput = false;
                tbCode.Text = value;
                _isUserInput = true;
            }
        }

        public IList<string> Lines
        {
            get { return tbCode.Lines; }
        }


        public CodeEditor()
        {
            InitializeComponent();
        }

        private void CodeEditor_Resize(object sender, EventArgs e)
        {
            tbCode.Height = this.Height - 62;
            tbCode.Width = this.Width - 6;

            tbName.Width = this.Width - 161;
            labelBy.Left = tbName.Width + tbName.Left;
            tbAuthor.Left = labelBy.Width + labelBy.Left;

            btWrite.Width = this.Width - 157;
        }

        private void tbCode_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (!_isUserInput)
                return;

            if (CodeChanged != null)
                CodeChanged.Invoke(this, CodeText);
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserInput)
                return;

            if (NameChanged != null)
                NameChanged.Invoke(this, CodeName);
        }

        private void cbConstant_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isUserInput)
                return;

            if (ConstantChanged != null)
                ConstantChanged.Invoke(this, Constant);
        }

        private void btWrite_Click(object sender, EventArgs e)
        {
            if (WriteActivated != null)
                WriteActivated.Invoke(this, null);
        }

        private void tbAuthor_TextChanged(object sender, EventArgs e)
        {
            if (!_isUserInput)
                return;

            if (AuthorChanged != null)
                AuthorChanged.Invoke(this, CodeAuthor);
        }
    }
}
