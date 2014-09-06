using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
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
        private string lastExcelSheetPath = "";
        
        public FlexMainWindow()
        {
            InitializeComponent();
            lblProgress.Text = "";
            cmbResultsType.Items.Add(SqlRunParameters.TO_INSERT_STATEMENTS);
            cmbResultsType.Items.Add(SqlRunParameters.TO_XML_SPREADSHEET);
            cmbResultsType.SelectedItem = SqlRunParameters.TO_INSERT_STATEMENTS;
            setUIState(false);
        }
    
        public void SetConnection(SqlConnectionStringBuilder theConnectionStringBuilder)
        {
            connStringBuilder = theConnectionStringBuilder;
            SetConnectionText();
        }

        private void SetConnectionText()
        {
            InvokeOnFormThread(() =>
            {
                lblConnectionInfo.Text = currentConnectionText();
            });
        }

        private string currentConnectionText()
        {
            if (Utils.IsValidConnectionStringBuilder(connStringBuilder))
            {
                return "Instance: " + connStringBuilder.DataSource + ", DB: " + connStringBuilder.InitialCatalog;
            }
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
                lastExcelSheetPath = "";
                sqlStopwatch = new Stopwatch();
                sqlStopwatch.Start();
                queryTimer.Enabled = true;
                setUIState(true);

                try
                {
                    var srp = new SqlRunParameters(connStringBuilder, getSqlToRun(), cmbResultsType.SelectedItem.ToString());
                    queryWorker.RunWorkerAsync(srp);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an exception when setting up the query run parameters.  "  + ex.Message + "\n\n" + ex.StackTrace);
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
            try
            {
                if (txtOutput.Text.Length > 0)
                {
                    Clipboard.SetText(txtOutput.Text);
                }
            }
            catch (Exception ex)
            {
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
            InvokeOnFormThread(() =>
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
                    if (srp.exceptionsText.Length > 0)
                    {
                        if (srp.resultsText.Length > 0)
                        {
                            txtOutput.Text = srp.exceptionsText.ToString() + "\r\n\r\n" + srp.resultsText.ToString();
                        }
                        else
                        {
                            drawExceptions(srp);
                        }
                    }
                    else
                    {
                        txtOutput.Text = srp.resultsText.ToString();
                    }
                    
                }
                else if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_XML_SPREADSHEET)
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
            }
            
            setProgressText(true);

            lblConnectionInfo.Text = currentConnectionText();
            
            setUIState(false);
        }

        private void drawExceptions(SqlRunParameters srp, string header = "")
        {
            InvokeOnFormThread(() => {
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

        private void TryToSaveSpreadsheet(SqlRunParameters srp)
        {
            string fileName = "";
            try
            {
                fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TSqlFlex" + DateTime.Now.ToString("_yyyyMMddTHHmmss") + ".xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception when attempting to find personal documents folder for current user.  Will not save file.  " + ex.Message);
                fileName = "";
            }

            if (fileName != "")
            {
                srp.saveOutputStreamTo(fileName);
                this.lastExcelSheetPath = fileName;
                InvokeOnFormThread(() =>
                {
                    txtOutput.Text = "--Results written to \"" + fileName + "\".\r\n\r\n--You can open this file in Excel.";
                });
            }
        }

        private void setUIState(bool queryIsRunning) {
            InvokeOnFormThread(() =>
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
                btnExcel.Enabled = (lastExcelSheetPath.Length > 0);
            });

        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            queryWorker.CancelAsync();
            cmdCancel.Enabled = false;
        }

        private void queryTimer_Tick(object sender, EventArgs e)
        {
            setProgressText(false);
        }

        private void InvokeOnFormThread(Action behavior)
        {
            if (IsHandleCreated && InvokeRequired)
            {
                Invoke(behavior);
            }
            else
            {
                behavior();
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            var Excel = new ExcelLauncher();
            if (Excel.ExcelFound)
            {
                Excel.Launch(this.lastExcelSheetPath);
            }
            else
            {
                MessageBox.Show(Excel.ExcelError, "T-SQL Flex couldn't launch Excel");
            }
        }
    }
}