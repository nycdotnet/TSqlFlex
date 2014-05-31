namespace TSqlFlex
{
    partial class FlexMainWindow
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
            this.btnCopyToNewWindow = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblConnectionInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSqlInput = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.cmdRunNRollback = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCopyToNewWindow
            // 
            this.btnCopyToNewWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyToNewWindow.Location = new System.Drawing.Point(979, 3);
            this.btnCopyToNewWindow.Name = "btnCopyToNewWindow";
            this.btnCopyToNewWindow.Size = new System.Drawing.Size(121, 23);
            this.btnCopyToNewWindow.TabIndex = 3;
            this.btnCopyToNewWindow.Text = "Copy to new window";
            this.btnCopyToNewWindow.UseVisualStyleBackColor = true;
            this.btnCopyToNewWindow.Visible = false;
            this.btnCopyToNewWindow.Click += new System.EventHandler(this.btnCopyToNewWindow_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelBottom);
            this.splitContainer1.Size = new System.Drawing.Size(1112, 548);
            this.splitContainer1.SplitterDistance = 272;
            this.splitContainer1.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblConnectionInfo);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.txtSqlInput);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1112, 272);
            this.panelTop.TabIndex = 0;
            // 
            // lblConnectionInfo
            // 
            this.lblConnectionInfo.AutoSize = true;
            this.lblConnectionInfo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionInfo.Location = new System.Drawing.Point(12, 49);
            this.lblConnectionInfo.Name = "lblConnectionInfo";
            this.lblConnectionInfo.Size = new System.Drawing.Size(452, 19);
            this.lblConnectionInfo.TabIndex = 4;
            this.lblConnectionInfo.Text = "Instance: [ServerNameAndInstance], DB: [DatabaseName]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(435, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "This is alpha software and will probably delete your database somehow.  For test " +
    "use only!!!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter a SQL command in the top section.  Click \"Run \'n\' Rollback\" to script out t" +
    "he data.";
            // 
            // txtSqlInput
            // 
            this.txtSqlInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSqlInput.Location = new System.Drawing.Point(0, 71);
            this.txtSqlInput.Multiline = true;
            this.txtSqlInput.Name = "txtSqlInput";
            this.txtSqlInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSqlInput.Size = new System.Drawing.Size(1112, 201);
            this.txtSqlInput.TabIndex = 0;
            this.txtSqlInput.WordWrap = false;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnCopyToNewWindow);
            this.panelBottom.Controls.Add(this.btnCopyToClipboard);
            this.panelBottom.Controls.Add(this.txtOutput);
            this.panelBottom.Controls.Add(this.cmdRunNRollback);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1112, 272);
            this.panelBottom.TabIndex = 1;
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyToClipboard.Location = new System.Drawing.Point(868, 3);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(105, 23);
            this.btnCopyToClipboard.TabIndex = 2;
            this.btnCopyToClipboard.Text = "&Copy to clipboard";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.BackColor = System.Drawing.SystemColors.Window;
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(0, 32);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(1112, 240);
            this.txtOutput.TabIndex = 4;
            this.txtOutput.WordWrap = false;
            // 
            // cmdRunNRollback
            // 
            this.cmdRunNRollback.Location = new System.Drawing.Point(3, 3);
            this.cmdRunNRollback.Name = "cmdRunNRollback";
            this.cmdRunNRollback.Size = new System.Drawing.Size(101, 23);
            this.cmdRunNRollback.TabIndex = 1;
            this.cmdRunNRollback.Text = "&Run \'n\' Rollback";
            this.cmdRunNRollback.UseVisualStyleBackColor = true;
            this.cmdRunNRollback.Click += new System.EventHandler(this.cmdRunNRollback_Click);
            // 
            // FlexMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FlexMainWindow";
            this.Size = new System.Drawing.Size(1112, 548);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCopyToNewWindow;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblConnectionInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSqlInput;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnCopyToClipboard;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button cmdRunNRollback;
    }
}
