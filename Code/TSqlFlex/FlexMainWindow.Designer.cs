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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnAbout = new System.Windows.Forms.Button();
            this.lblConnectionInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSqlInput = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnTxt = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.cmbResultsType = new System.Windows.Forms.ComboBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.queryProgress = new System.Windows.Forms.ProgressBar();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.cmdRunNRollback = new System.Windows.Forms.Button();
            this.queryWorker = new System.ComponentModel.BackgroundWorker();
            this.queryTimer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnAbout);
            this.panelTop.Controls.Add(this.lblConnectionInfo);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.txtSqlInput);
            this.panelTop.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1112, 199);
            this.panelTop.TabIndex = 0;
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbout.Location = new System.Drawing.Point(1048, 3);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(61, 22);
            this.btnAbout.TabIndex = 6;
            this.btnAbout.Text = "&About...";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // lblConnectionInfo
            // 
            this.lblConnectionInfo.AutoSize = true;
            this.lblConnectionInfo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionInfo.Location = new System.Drawing.Point(3, 4);
            this.lblConnectionInfo.Name = "lblConnectionInfo";
            this.lblConnectionInfo.Size = new System.Drawing.Size(452, 19);
            this.lblConnectionInfo.TabIndex = 4;
            this.lblConnectionInfo.Text = "Instance: [ServerNameAndInstance], DB: [DatabaseName]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter a SQL command in the top section.  Click \"Run \'n\' Rollback\" to script out t" +
    "he data.";
            // 
            // txtSqlInput
            // 
            this.txtSqlInput.AllowDrop = true;
            this.txtSqlInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSqlInput.Location = new System.Drawing.Point(0, 48);
            this.txtSqlInput.Multiline = true;
            this.txtSqlInput.Name = "txtSqlInput";
            this.txtSqlInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSqlInput.Size = new System.Drawing.Size(1112, 151);
            this.txtSqlInput.TabIndex = 0;
            this.txtSqlInput.WordWrap = false;
            this.txtSqlInput.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtSqlInput_DragDrop);
            this.txtSqlInput.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtSqlInput_DragEnter);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnTxt);
            this.panelBottom.Controls.Add(this.btnExcel);
            this.panelBottom.Controls.Add(this.cmbResultsType);
            this.panelBottom.Controls.Add(this.lblProgress);
            this.panelBottom.Controls.Add(this.cmdCancel);
            this.panelBottom.Controls.Add(this.queryProgress);
            this.panelBottom.Controls.Add(this.btnCopyToClipboard);
            this.panelBottom.Controls.Add(this.txtOutput);
            this.panelBottom.Controls.Add(this.cmdRunNRollback);
            this.panelBottom.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1112, 347);
            this.panelBottom.TabIndex = 1;
            // 
            // btnTxt
            // 
            this.btnTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTxt.Location = new System.Drawing.Point(831, 5);
            this.btnTxt.Name = "btnTxt";
            this.btnTxt.Size = new System.Drawing.Size(75, 23);
            this.btnTxt.TabIndex = 12;
            this.btnTxt.Text = "Open as .&txt";
            this.btnTxt.UseVisualStyleBackColor = true;
            this.btnTxt.Click += new System.EventHandler(this.btnTxt_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(912, 5);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(86, 23);
            this.btnExcel.TabIndex = 7;
            this.btnExcel.Text = "Open in &Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // cmbResultsType
            // 
            this.cmbResultsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResultsType.FormattingEnabled = true;
            this.cmbResultsType.Location = new System.Drawing.Point(3, 5);
            this.cmbResultsType.Name = "cmbResultsType";
            this.cmbResultsType.Size = new System.Drawing.Size(192, 21);
            this.cmbResultsType.TabIndex = 11;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(460, 10);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(41, 13);
            this.lblProgress.TabIndex = 9;
            this.lblProgress.Text = "Ready.";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Enabled = false;
            this.cmdCancel.Location = new System.Drawing.Point(379, 5);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Ca&ncel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // queryProgress
            // 
            this.queryProgress.Location = new System.Drawing.Point(308, 10);
            this.queryProgress.Name = "queryProgress";
            this.queryProgress.Size = new System.Drawing.Size(65, 13);
            this.queryProgress.TabIndex = 6;
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyToClipboard.Location = new System.Drawing.Point(1004, 5);
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
            this.txtOutput.Size = new System.Drawing.Size(1112, 315);
            this.txtOutput.TabIndex = 4;
            this.txtOutput.WordWrap = false;
            // 
            // cmdRunNRollback
            // 
            this.cmdRunNRollback.Location = new System.Drawing.Point(201, 5);
            this.cmdRunNRollback.Name = "cmdRunNRollback";
            this.cmdRunNRollback.Size = new System.Drawing.Size(101, 23);
            this.cmdRunNRollback.TabIndex = 1;
            this.cmdRunNRollback.Text = "&Run \'n\' Rollback";
            this.cmdRunNRollback.UseVisualStyleBackColor = true;
            this.cmdRunNRollback.Click += new System.EventHandler(this.cmdRunNRollback_Click);
            // 
            // queryWorker
            // 
            this.queryWorker.WorkerReportsProgress = true;
            this.queryWorker.WorkerSupportsCancellation = true;
            this.queryWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.queryWorker_DoWork);
            this.queryWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.queryWorker_ProgressChanged);
            this.queryWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.queryWorker_RunWorkerCompleted);
            // 
            // queryTimer
            // 
            this.queryTimer.Tick += new System.EventHandler(this.queryTimer_Tick);
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

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblConnectionInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSqlInput;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnCopyToClipboard;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button cmdRunNRollback;
        private System.ComponentModel.BackgroundWorker queryWorker;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ProgressBar queryProgress;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Timer queryTimer;
        private System.Windows.Forms.ComboBox cmbResultsType;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnTxt;
    }
}
