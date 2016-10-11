namespace LegacyCode.UI
{
    partial class CodeEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeEditor));
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbConstant = new System.Windows.Forms.CheckBox();
            this.tbCode = new FastColoredTextBoxNS.FastColoredTextBox();
            this.btWrite = new System.Windows.Forms.Button();
            this.tbAuthor = new System.Windows.Forms.TextBox();
            this.labelBy = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).BeginInit();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(3, 3);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(219, 20);
            this.tbName.TabIndex = 0;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // cbConstant
            // 
            this.cbConstant.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbConstant.Location = new System.Drawing.Point(3, 29);
            this.cbConstant.Name = "cbConstant";
            this.cbConstant.Size = new System.Drawing.Size(145, 23);
            this.cbConstant.TabIndex = 1;
            this.cbConstant.Text = "Constant Write";
            this.cbConstant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbConstant.UseVisualStyleBackColor = true;
            this.cbConstant.CheckedChanged += new System.EventHandler(this.cbConstant_CheckedChanged);
            // 
            // tbCode
            // 
            this.tbCode.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.tbCode.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.tbCode.BackBrush = null;
            this.tbCode.CharHeight = 14;
            this.tbCode.CharWidth = 8;
            this.tbCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCode.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tbCode.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.tbCode.IsReplaceMode = false;
            this.tbCode.Location = new System.Drawing.Point(3, 59);
            this.tbCode.Name = "tbCode";
            this.tbCode.Paddings = new System.Windows.Forms.Padding(0);
            this.tbCode.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tbCode.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tbCode.ServiceColors")));
            this.tbCode.Size = new System.Drawing.Size(374, 199);
            this.tbCode.TabIndex = 2;
            this.tbCode.Text = "fastColoredTextBox1";
            this.tbCode.Zoom = 100;
            this.tbCode.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.tbCode_TextChanged);
            // 
            // btWrite
            // 
            this.btWrite.Location = new System.Drawing.Point(154, 29);
            this.btWrite.Name = "btWrite";
            this.btWrite.Size = new System.Drawing.Size(223, 23);
            this.btWrite.TabIndex = 3;
            this.btWrite.Text = "Write";
            this.btWrite.UseVisualStyleBackColor = true;
            this.btWrite.Click += new System.EventHandler(this.btWrite_Click);
            // 
            // tbAuthor
            // 
            this.tbAuthor.Location = new System.Drawing.Point(240, 3);
            this.tbAuthor.Name = "tbAuthor";
            this.tbAuthor.Size = new System.Drawing.Size(137, 20);
            this.tbAuthor.TabIndex = 4;
            this.tbAuthor.TextChanged += new System.EventHandler(this.tbAuthor_TextChanged);
            // 
            // labelBy
            // 
            this.labelBy.AutoSize = true;
            this.labelBy.Location = new System.Drawing.Point(222, 6);
            this.labelBy.Name = "labelBy";
            this.labelBy.Size = new System.Drawing.Size(18, 13);
            this.labelBy.TabIndex = 5;
            this.labelBy.Text = "by";
            // 
            // CodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelBy);
            this.Controls.Add(this.tbAuthor);
            this.Controls.Add(this.btWrite);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.cbConstant);
            this.Controls.Add(this.tbName);
            this.Name = "CodeEditor";
            this.Size = new System.Drawing.Size(380, 261);
            this.Resize += new System.EventHandler(this.CodeEditor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.CheckBox cbConstant;
        private FastColoredTextBoxNS.FastColoredTextBox tbCode;
        private System.Windows.Forms.Button btWrite;
        private System.Windows.Forms.TextBox tbAuthor;
        private System.Windows.Forms.Label labelBy;
    }
}
