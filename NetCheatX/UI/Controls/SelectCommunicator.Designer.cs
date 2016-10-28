namespace NetCheatX.UI.Controls
{
    partial class SelectCommunicator
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbComs = new System.Windows.Forms.ListBox();
            this.lbDesc = new System.Windows.Forms.Label();
            this.btSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbComs
            // 
            this.lbComs.FormattingEnabled = true;
            this.lbComs.Location = new System.Drawing.Point(12, 12);
            this.lbComs.Name = "lbComs";
            this.lbComs.Size = new System.Drawing.Size(178, 303);
            this.lbComs.TabIndex = 0;
            this.lbComs.SelectedIndexChanged += new System.EventHandler(this.lbComs_SelectedIndexChanged);
            // 
            // lbDesc
            // 
            this.lbDesc.Location = new System.Drawing.Point(196, 12);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(388, 277);
            this.lbDesc.TabIndex = 1;
            this.lbDesc.Text = "label1";
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(196, 292);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(388, 23);
            this.btSelect.TabIndex = 2;
            this.btSelect.Text = "Select";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // SelectCommunicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 321);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.lbDesc);
            this.Controls.Add(this.lbComs);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(612, 360);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(612, 360);
            this.Name = "SelectCommunicator";
            this.Text = "Select Platform Communicator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbComs;
        private System.Windows.Forms.Label lbDesc;
        private System.Windows.Forms.Button btSelect;
    }
}