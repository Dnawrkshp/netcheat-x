using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetCheatX.Core;

namespace LegacyCode.UI
{
    public partial class Editor : Form
    {
        private IPluginHost _host = null;
        private string _text = null;

        private List<Code.Processor> _processors = null;

        public Editor(IPluginHost host, string text)
        {
            _host = host;
            _text = text;

            InitializeComponent();


            Text = text;

            // Initialize new code processor
            _processors = new List<Code.Processor>();
        }

        public void Cleanup()
        {
            _host = null;
            _text = null;

            if (_processors != null)
            {
                foreach (Code.Processor proc in _processors)
                    proc.Dispose();
                _processors.Clear();
                _processors = null;
            }
        }

        public void AddCode(string name, string author, string code, bool constant = false)
        {
            Code.Processor processor = new Code.Processor(_host);
            processor.Name = name;
            processor.Author = author;
            processor.CodeText = code;
            processor.Constant = constant;

            lbCodes.Items.Add(name);
            _processors.Add(processor);
        }

        #region Codes List Box

        private void lbCodes_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(e.Location);
            }
        }

        private void lbCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeEditor1.Enabled = lbCodes.SelectedIndex >= 0;

            if (codeEditor1.Enabled)
            {
                codeEditor1.CodeName = _processors[lbCodes.SelectedIndex].Name;
                codeEditor1.CodeText = _processors[lbCodes.SelectedIndex].CodeText;
                codeEditor1.Constant = _processors[lbCodes.SelectedIndex].Constant;
            }
        }

        #endregion

        #region Context Menu

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCode("NEW CODE", _host.AuthorDefault, "");
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            _processors.RemoveAt(lbCodes.SelectedIndex);
            lbCodes.Items.RemoveAt(lbCodes.SelectedIndex);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            Clipboard.SetDataObject(_processors[lbCodes.SelectedIndex], false);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object dataObject = Clipboard.GetDataObject();
            if (dataObject is Code.Processor)
            {
                AddCode((dataObject as Code.Processor).Name,
                    (dataObject as Code.Processor).Author,
                    (dataObject as Code.Processor).CodeText,
                    (dataObject as Code.Processor).Constant);
            }
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            AddCode(_processors[lbCodes.SelectedIndex].Name,
                _processors[lbCodes.SelectedIndex].Author,
                _processors[lbCodes.SelectedIndex].CodeText,
                _processors[lbCodes.SelectedIndex].Constant);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lbCodes.Items.Clear();

            if (_processors != null)
            {
                foreach (Code.Processor proc in _processors)
                    proc.Dispose();
                _processors.Clear();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Code Editor

        private void codeEditor1_WriteActivated(object sender, EventArgs e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            _processors[lbCodes.SelectedIndex].Execute();
        }

        private void codeEditor1_NameChanged(object sender, string e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            _processors[lbCodes.SelectedIndex].Name = codeEditor1.CodeName;
        }

        private void codeEditor1_ConstantChanged(object sender, bool e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            _processors[lbCodes.SelectedIndex].Constant = codeEditor1.Constant;
        }

        private void codeEditor1_CodeChanged(object sender, string e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            _processors[lbCodes.SelectedIndex].CodeText = codeEditor1.CodeText;
        }

        private void codeEditor1_AuthorChanged(object sender, string e)
        {
            if (lbCodes.SelectedIndex < 0)
                return;

            _processors[lbCodes.SelectedIndex].Author = codeEditor1.CodeAuthor;
        }

        #endregion

    }
}
