using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TSqlFlex.Core;

namespace TSqlFlex
{
    [ComVisible(true)]
    public partial class FlexMainWindow : UserControl
    {
        private SqlConnectionStringBuilder connStringBuilder = null;
        private Stopwatch sqlStopwatch = null;
        private string progressText = "";
        private string lastExportedFilePath = "";
        private Logging logger;
        private uint completedResultsCount = 0;
        
        public FlexMainWindow(Logging instantiatedLogger)
        {
            logger = instantiatedLogger;
            logger.LogVerbose("Initializing FlexMainWindow");
            InitializeComponent();
            lblProgress.Text = "";
            cmbResultsType.Items.Add(SqlRunParameters.TO_INSERT_STATEMENTS);
            cmbResultsType.Items.Add(SqlRunParameters.TO_XML_SPREADSHEET);
            cmbResultsType.Items.Add(SqlRunParameters.TO_CSV);
            cmbResultsType.Items.Add(SqlRunParameters.TO_CSHARP);
            cmbResultsType.SelectedItem = SqlRunParameters.TO_INSERT_STATEMENTS;
            setUIState(false);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var control = FindFocusedControl(this);

            if (keyData == (Keys.Control | Keys.A))
            {
                if (control == txtSqlInput || control == txtOutput)
                {
                    var txt = control as TextBox;
                    txt.SelectAll();
                }
                return true;
            }
            else if (keyData == (Keys.Control | Keys.C))
            {
                if (control == txtSqlInput || control == txtOutput)
                {
                    var txt = control as TextBox;
                    CopyThisTextOrShowErrorMessageBox(txt.SelectedText);
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        
        }

        //Thanks!  http://stackoverflow.com/questions/435433/what-is-the-preferred-way-to-find-focused-control-in-winforms-app/
        public static Control FindFocusedControl(Control control)
        {
            var container = control as ContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as ContainerControl;
            }
            return control;
        }
    
        public void SetConnection(SqlConnectionStringBuilder theConnectionStringBuilder)
        {
            logger.LogVerbose("FlexMainWindow.SetConnection");
            connStringBuilder = theConnectionStringBuilder;
            SetConnectionText();
            logger.LogVerbose("FlexMainWindow.SetConnection Complete");
        }

        private void SetConnectionText()
        {
            logger.LogVerbose("FlexMainWindow.SetConnectionText");
            InvokeFireAndForgetOnFormThread(() =>
            {
                lblConnectionInfo.Text = currentConnectionText();
            });
            logger.LogVerbose("FlexMainWindow.SetConnectionText Complete");
        }

        private string currentConnectionText()
        {
            logger.LogVerbose("FlexMainWindow.currentConnectionText");

            if (Utils.IsValidConnectionStringBuilder(connStringBuilder))
            {
                logger.LogVerbose("FlexMainWindow.currentConnectionText valid builder");
                return "Instance: " + connStringBuilder.DataSource + ", DB: " + connStringBuilder.InitialCatalog;
            }
            logger.LogVerbose("FlexMainWindow.currentConnectionText not connected");
            return "Not connected.";
        }
        
        private void cmdRunNRollback_Click(object sender, EventArgs e)
        {
            if (!Utils.IsValidConnectionStringBuilder(connStringBuilder))
            {
                MessageBox.Show("Select the target database in the object tree first.");
                return;
            }

            if (!queryWorker.IsBusy) {
                lastExportedFilePath = "";
                sqlStopwatch = new Stopwatch();
                sqlStopwatch.Start();
                queryTimer.Enabled = true;
                setUIState(true);

                try
                {
                    var srp = new SqlRunParameters(connStringBuilder, getSqlToRun(), cmbResultsType.SelectedItem.ToString());
                    srp.completedResultsCount = completedResultsCount;
                    queryWorker.RunWorkerAsync(srp);
                }
                catch (Exception ex)
                {
                    string error = "There was an exception when setting up the query run parameters.  " + ex.Message;
                    logger.Log(error + " " + ex.StackTrace);
                    MessageBox.Show(error);
                    sqlStopwatch.Stop();
                    queryTimer.Enabled = false;
                    setUIState(false);
                }
            }
        }

        private string getSqlToRun()
        {
            if (txtSqlInput.SelectionLength > 0)
            {
                return txtSqlInput.SelectedText;
            }
            return txtSqlInput.Text;
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            CopyThisTextOrShowErrorMessageBox(txtOutput.Text);
        }

        private void CopyThisTextOrShowErrorMessageBox(string theTextToCopy) {
            try
            {
                if (!String.IsNullOrEmpty(theTextToCopy))
                {
                    Clipboard.SetText(theTextToCopy);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                logger.Log(ex.StackTrace);
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        private void btnCopyToNewWindow_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet implemented.");
        }

        private void queryWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            QueryWorker.DoSqlQueryWork(e,queryWorker);
        }

        private void queryWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e != null)
            {
                if (e.UserState is string && e.UserState != null)
                {
                    progressText = (string)e.UserState;
                    setProgressText( (e.ProgressPercentage == 100) );
                }

                queryProgress.Value = e.ProgressPercentage;
            }
        }

        private void setProgressText(bool isComplete)
        {
            InvokeFireAndForgetOnFormThread(() =>
            {
                lblProgress.Text = getFormattedElapsedSqlStopwatchTime(isComplete) + " " + progressText;
                lblProgress.Refresh();
            });
        }

        private string getFormattedElapsedSqlStopwatchTime(bool includeMilliseconds)
        {
            if (sqlStopwatch == null)
            {
                return "";
            }
            TimeSpan t = sqlStopwatch.Elapsed;

            if (includeMilliseconds)
            {
                return String.Format("{0}:{1}:{2}.{3}",
                        t.Hours.ToString(),
                        t.Minutes.ToString().PadLeft(2, '0'),
                        t.Seconds.ToString().PadLeft(2, '0'),
                        t.Milliseconds.ToString().PadLeft(3, '0').TrimEnd('0'));
            }

            return String.Format("{0}:{1}:{2}",
                        t.Hours.ToString(),
                        t.Minutes.ToString().PadLeft(2, '0'),
                        t.Seconds.ToString().PadLeft(2, '0'));
        }

        private void queryWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            queryTimer.Enabled = false;
            queryProgress.Value = 100;
            progressText = "Drawing results...";  //if there is a large amount of data, drawing it in the textbox can take a long time.
            sqlStopwatch.Stop();
            setProgressText(true); //bug: This includes the time it took to read all of the results, etc.  Should technically stop after data finishes coming in from SQL

            var srp = (SqlRunParameters)e.Result;

            completedResultsCount += srp.completedResultsCount - completedResultsCount;

            srp.flushAndCloseOutputStreamIfNeeded();
            
            if (e.Cancelled)
            {
                progressText = "Cancelled.";
                txtOutput.Text = "--Query cancelled.";
            }
            else if (e.Error != null)
            {
                progressText = "Finished with error.";
                //todo: make this more consistent with what happens from SQL syntax errors, etc.
                txtOutput.Text = "--Error occurred while processing query.\r\n\r\n/* " + e.Error.Message + "\r\n*/";
            }
            else
            {
                progressText = "Complete.";
                if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_INSERT_STATEMENTS)
                {
                    RenderInsertStatements(srp);
                }
                else if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_XML_SPREADSHEET)
                {
                    RenderXmlSpreadsheet(srp);
                }
                else if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_CSV)
                {
                    RenderCsv(srp);
                }
                else if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_CSHARP)
                {
                    RenderCSharp(srp);
                }
            }
            
            setProgressText(true);

            lblConnectionInfo.Text = currentConnectionText();
            
            setUIState(false);
        }

        private void RenderCsv(SqlRunParameters srp)
        {
            if (srp.worksheetIsValid)
            {
                TryToSaveCSVs(srp);
            }
            else
            {
                drawExceptions(srp);
            }
        }

        private void RenderXmlSpreadsheet(SqlRunParameters srp)
        {
            if (srp.worksheetIsValid)
            {
                TryToSaveSpreadsheet(srp);
            }
            else
            {
                drawExceptions(srp);
            }
        }

        private void RenderCSharp(SqlRunParameters srp)
        {
            bool success = false;
            try
            {
                if (srp.exceptionsText.Length > 0)
                {
                    if (srp.resultsText.Length > 0)
                    {
                        txtOutput.Text = srp.exceptionsText.ToString() + "\r\n\r\n" + srp.resultsText.ToString();
                        success = true;
                    }
                    else
                    {
                        drawExceptions(srp);
                        success = true;
                    }
                }
                else
                {
                    txtOutput.Text = srp.resultsText.ToString();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                srp.resultsText = null;
                srp.exceptionsText.Append("\r\n\r\n/*\r\n\r\nException while attempting to display results: ");
                srp.exceptionsText.Append(ex.Message);
                srp.exceptionsText.Append("\r\n\r\n");
                srp.exceptionsText.Append(ex.StackTrace);
                srp.exceptionsText.Append("\r\n*/");
            }

            if (!success)
            {
                GC.Collect();
                txtOutput.Text = srp.exceptionsText.ToString();
            }
        }


        private void RenderInsertStatements(SqlRunParameters srp)
        {
            bool success = false;
            try
            {
                if (srp.exceptionsText.Length > 0)
                {
                    if (srp.resultsText.Length > 0)
                    {
                        txtOutput.Text = srp.exceptionsText.ToString() + "\r\n\r\n" + srp.resultsText.ToString();
                        success = true;
                    }
                    else
                    {
                        drawExceptions(srp);
                        success = true;
                    }
                }
                else
                {
                    txtOutput.Text = srp.resultsText.ToString();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                srp.resultsText = null;
                srp.exceptionsText.Append("\r\n\r\n/*\r\n\r\nException while attempting to display results: ");
                srp.exceptionsText.Append(ex.Message);
                srp.exceptionsText.Append("\r\n\r\n");
                srp.exceptionsText.Append(ex.StackTrace);
                srp.exceptionsText.Append("\r\n*/");
            }

            if (!success)
            {
                GC.Collect();
                txtOutput.Text = srp.exceptionsText.ToString();
            }
        }

        private void drawExceptions(SqlRunParameters srp, string header = "")
        {
            InvokeFireAndForgetOnFormThread(() => {
                    if (header == "")
                    {
                        txtOutput.Text = srp.exceptionsText.ToString();
                    }
                    else
                    {
                        txtOutput.Text = header + "\r\n\r\n" + srp.exceptionsText.ToString();
                    }
                });
        }

        private void TryToSaveCSVs(SqlRunParameters srp)
        {
            txtOutput.Text = "";
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string timestamp = DateTime.Now.ToString("_yyyyMMddTHHmmss");
            for (int csvIndex = 0; csvIndex < srp.outputFiles.Count; csvIndex += 1)
            {
                string fileName = "";
                try
                {
                    fileName = Path.Combine(personalFolder, "TSqlFlex" + timestamp +
                        (srp.outputFiles.Count > 1 ? "_" + (csvIndex+1).ToString() : "") + ".csv");
                }
                catch (Exception ex)
                {
                    string error = "Exception when attempting to find personal documents folder for current user.  Will not save file.";
                    logger.Log(error + " " + ex.Message);
                    logger.Log(ex.StackTrace);
                    MessageBox.Show(error + "  " + ex.Message);
                    fileName = "";
                    break;
                }

                if (fileName != "")
                {
                    srp.saveOutputStreamTo(srp.outputFiles[csvIndex], fileName);
                    this.lastExportedFilePath = fileName;
                    InvokeFireAndForgetOnFormThread(() =>
                    {
                        txtOutput.Text += "--Results written to \"" + fileName + "\".\r\n--You can open this file in your text editor.\r\n\r\n";
                    });
                }
            }
        }

        private void TryToSaveSpreadsheet(SqlRunParameters srp)
        {
            string fileName = "";
            try
            {
                fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TSqlFlex" + DateTime.Now.ToString("_yyyyMMddTHHmmss") + ".xml");
            }
            catch (Exception ex)
            {
                string error = "Exception when attempting to find personal documents folder for current user.  Will not save file.";
                logger.Log(error + " " + ex.Message);
                logger.Log(ex.StackTrace);
                MessageBox.Show(error + "  " + ex.Message);
                fileName = "";
            }

            if (fileName != "")
            {
                srp.saveOutputStreamTo(srp.outputFiles[0], fileName);
                this.lastExportedFilePath = fileName;
                InvokeFireAndForgetOnFormThread(() =>
                {
                    txtOutput.Text = "--Results written to \"" + fileName + "\".\r\n--You can open this file in Excel.\r\n\r\n";
                });
            }
        }

        private void setUIState(bool queryIsRunning) {
            logger.LogVerbose("FlexMainWindow.setUIState");
            InvokeFireAndForgetOnFormThread(() =>
            {
                cmdCancel.Enabled = queryIsRunning;
                cmdRunNRollback.Enabled = !queryIsRunning;
                if (queryIsRunning)
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                }
                btnExcel.Enabled = (lastExportedFilePath.Length > 0);
                btnTxt.Enabled = (lastExportedFilePath.Length > 0);
            });
            logger.LogVerbose("FlexMainWindow.setUIState complete");
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            queryWorker.CancelAsync();
            cmdCancel.Enabled = false;
        }

        private void queryTimer_Tick(object sender, EventArgs e)
        {
            logger.LogVerbose("FlexMainWindow.queryTimerTick");
            setProgressText(false);
            logger.LogVerbose("FlexMainWindow.queryTimerTick Complete");
        }

        private void InvokeFireAndForgetOnFormThread(Action behavior)
        {
            logger.LogVerbose("FlexMainWindow.InvokeOnFormThread");
            if (IsHandleCreated && InvokeRequired)
            {
                //Thanks to http://stackoverflow.com/questions/229554/whats-the-difference-between-invoke-and-begininvoke/229558#229558 
                logger.LogVerbose("FlexMainWindow.InvokeOnFormThread (attempting to invoke)");
                BeginInvoke(behavior);
                logger.LogVerbose("FlexMainWindow.InvokeOnFormThread (finished invoke)");
            }
            else
            {
                logger.LogVerbose("FlexMainWindow.InvokeOnFormThread (attempting to do behavior)");
                behavior();
                logger.LogVerbose("FlexMainWindow.InvokeOnFormThread (finished behavior)");
            }
            logger.LogVerbose("FlexMainWindow.InvokeOnFormThread Complete");
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            var Excel = new ExcelLauncher();
            if (Excel.ProgramFound)
            {
                Excel.Launch(this.lastExportedFilePath);
            }
            else
            {
                MessageBox.Show(Excel.ProgramError, "T-SQL Flex couldn't launch Excel");
            }
        }

        private void txtSqlInput_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string theObjectName = e.Data.GetData(DataFormats.Text).ToString();
                Point pointOnTextbox = txtSqlInput.PointToClient(new Point(e.X, e.Y));
                int charIndex = txtSqlInput.GetCharIndexFromPosition(pointOnTextbox);
                txtSqlInput.Text = txtSqlInput.Text.Insert(charIndex, FieldScripting.EscapeObjectNames(theObjectName));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not drag and drop.\r\n\r\n" + ex.Message, "T-SQL Flex");
            }
        }

        private void txtSqlInput_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            { 
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnTxt_Click(object sender, EventArgs e)
        {
            var Txt = new TxtLauncher();
            if (Txt.ProgramFound)
            {
                Txt.Launch(this.lastExportedFilePath);
            }
            else
            {
                MessageBox.Show(Txt.ProgramError, "T-SQL Flex couldn't launch your default text editor.");
            }
        }
    }
}