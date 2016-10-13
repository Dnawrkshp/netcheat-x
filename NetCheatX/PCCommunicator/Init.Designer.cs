namespace PCCommunicator
{
    partial class Init
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
            this.listProcess = new System.Windows.Forms.ListBox();
            this.butAttach = new System.Windows.Forms.Button();
            this.butRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listProcess
            // 
            this.listProcess.FormattingEnabled = true;
            this.listProcess.Location = new System.Drawing.Point(12, 12);
            this.listProcess.Name = "listProcess";
            this.listProcess.Size = new System.Drawing.Size(260, 212);
            this.listProcess.TabIndex = 0;
            // 
            // butAttach
            // 
            this.butAttach.Location = new System.Drawing.Point(12, 230);
            this.butAttach.Name = "butAttach";
            this.butAttach.Size = new System.Drawing.Size(122, 23);
            this.butAttach.TabIndex = 1;
            this.butAttach.Text = "Attach";
            this.butAttach.UseVisualStyleBackColor = true;
            this.butAttach.Click += new System.EventHandler(this.butAttach_Click);
            // 
            // butRefresh
            // 
            this.butRefresh.Location = new System.Drawing.Point(150, 230);
            this.butRefresh.Name = "butRefresh";
            this.butRefresh.Size = new System.Drawing.Size(122, 23);
            this.butRefresh.TabIndex = 2;
            this.butRefresh.Text = "Refresh";
            this.butRefresh.UseVisualStyleBackColor = true;
            this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
            // 
            // Init
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.butRefresh);
            this.Controls.Add(this.butAttach);
            this.Controls.Add(this.listProcess);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "Init";
            this.Text = " Attach Process";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listProcess;
        private System.Windows.Forms.Button butAttach;
        private System.Windows.Forms.Button butRefresh;
    }
}