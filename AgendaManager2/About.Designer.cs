namespace AgendaManager2
{
    partial class About
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelProgramVersion = new System.Windows.Forms.Label();
            this.labelLibraryVersion = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelProgramVersion, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelLibraryVersion, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.okButton, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 11);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(367, 81);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelProgramVersion
            // 
            this.labelProgramVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProgramVersion.Location = new System.Drawing.Point(8, 0);
            this.labelProgramVersion.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
            this.labelProgramVersion.MaximumSize = new System.Drawing.Size(0, 21);
            this.labelProgramVersion.Name = "labelProgramVersion";
            this.labelProgramVersion.Size = new System.Drawing.Size(355, 21);
            this.labelProgramVersion.TabIndex = 0;
            this.labelProgramVersion.Text = "Agenda Manager 2 Version";
            this.labelProgramVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLibraryVersion
            // 
            this.labelLibraryVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLibraryVersion.Location = new System.Drawing.Point(8, 21);
            this.labelLibraryVersion.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
            this.labelLibraryVersion.MaximumSize = new System.Drawing.Size(0, 21);
            this.labelLibraryVersion.Name = "labelLibraryVersion";
            this.labelLibraryVersion.Size = new System.Drawing.Size(355, 21);
            this.labelLibraryVersion.TabIndex = 21;
            this.labelLibraryVersion.Text = "Agenda Library Version";
            this.labelLibraryVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(263, 49);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 28);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // About
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 103);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelProgramVersion;
        private System.Windows.Forms.Label labelLibraryVersion;
        private System.Windows.Forms.Button okButton;
    }
}
