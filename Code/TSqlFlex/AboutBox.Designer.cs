namespace TSqlFlex
{
    partial class AboutBox
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
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblKudos = new System.Windows.Forms.Label();
            this.btnTwitter = new System.Windows.Forms.Button();
            this.lblIssue = new System.Windows.Forms.Label();
            this.btnIssues = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReleases = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.Location = new System.Drawing.Point(12, 9);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(72, 13);
            this.labelProductName.TabIndex = 2;
            this.labelProductName.Text = "ProductName";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(12, 33);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "Version";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new System.Drawing.Point(12, 58);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(51, 13);
            this.labelCopyright.TabIndex = 4;
            this.labelCopyright.Text = "Copyright";
            // 
            // labelCompanyName
            // 
            this.labelCompanyName.AutoSize = true;
            this.labelCompanyName.Location = new System.Drawing.Point(12, 83);
            this.labelCompanyName.Name = "labelCompanyName";
            this.labelCompanyName.Size = new System.Drawing.Size(79, 13);
            this.labelCompanyName.TabIndex = 5;
            this.labelCompanyName.Text = "CompanyName";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(201, 215);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblKudos
            // 
            this.lblKudos.AutoSize = true;
            this.lblKudos.Location = new System.Drawing.Point(12, 122);
            this.lblKudos.Name = "lblKudos";
            this.lblKudos.Size = new System.Drawing.Size(153, 13);
            this.lblKudos.TabIndex = 7;
            this.lblKudos.Text = "Tell Steve you like T-SQL Flex:";
            // 
            // btnTwitter
            // 
            this.btnTwitter.ForeColor = System.Drawing.Color.Blue;
            this.btnTwitter.Location = new System.Drawing.Point(201, 116);
            this.btnTwitter.Name = "btnTwitter";
            this.btnTwitter.Size = new System.Drawing.Size(80, 25);
            this.btnTwitter.TabIndex = 8;
            this.btnTwitter.Text = "@nycdotnet";
            this.btnTwitter.UseVisualStyleBackColor = true;
            this.btnTwitter.Click += new System.EventHandler(this.btnTwitter_Click);
            // 
            // lblIssue
            // 
            this.lblIssue.AutoSize = true;
            this.lblIssue.Location = new System.Drawing.Point(12, 153);
            this.lblIssue.Name = "lblIssue";
            this.lblIssue.Size = new System.Drawing.Size(85, 13);
            this.lblIssue.TabIndex = 9;
            this.lblIssue.Text = "Report an Issue:";
            // 
            // btnIssues
            // 
            this.btnIssues.ForeColor = System.Drawing.Color.Blue;
            this.btnIssues.Location = new System.Drawing.Point(167, 147);
            this.btnIssues.Name = "btnIssues";
            this.btnIssues.Size = new System.Drawing.Size(113, 25);
            this.btnIssues.TabIndex = 10;
            this.btnIssues.Text = "TSqlFlex/issues";
            this.btnIssues.UseVisualStyleBackColor = true;
            this.btnIssues.Click += new System.EventHandler(this.btnIssues_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Check for an update:";
            // 
            // btnReleases
            // 
            this.btnReleases.ForeColor = System.Drawing.Color.Blue;
            this.btnReleases.Location = new System.Drawing.Point(168, 178);
            this.btnReleases.Name = "btnReleases";
            this.btnReleases.Size = new System.Drawing.Size(113, 25);
            this.btnReleases.TabIndex = 12;
            this.btnReleases.Text = "TSqlFlex/releases";
            this.btnReleases.UseVisualStyleBackColor = true;
            this.btnReleases.Click += new System.EventHandler(this.btnReleases_Click);
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 250);
            this.Controls.Add(this.btnReleases);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIssues);
            this.Controls.Add(this.lblIssue);
            this.Controls.Add(this.btnTwitter);
            this.Controls.Add(this.lblKudos);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelCompanyName);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelProductName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblKudos;
        private System.Windows.Forms.Button btnTwitter;
        private System.Windows.Forms.Label lblIssue;
        private System.Windows.Forms.Button btnIssues;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReleases;

    }
}
