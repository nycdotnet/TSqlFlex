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
        
        public FlexMainWindow()
        {
            InitializeComponent();
            lblVersion.Text = TSqlFlex.Core.Info.Version();
            lblProgress.Text = "";
            cmbResultsType.Items.Add(SqlRunParameters.TO_INSERT_STATEMENTS);
            cmbResultsType.Items.Add(SqlRunParameters.TO_XML_SPREADSHEET);
            cmbResultsType.SelectedItem = SqlRunParameters.TO_INSERT_STATEMENTS;
        }
    
        public void SetConnection(SqlConnectionStringBuilder theConnectionStringBuilder)
        {
            connStringBuilder = theConnectionStringBuilder;
            SetConnectionText();
        }

        private void SetConnectionText()
        {
            //bug: got cross thread error here. need to check for access for any UI updates.
            lblConnectionInfo.Text = currentConnectionText();
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
                sqlStopwatch = new Stopwatch();
                sqlStopwatch.Start();
                queryTimer.Enabled = true;

                cmdRunNRollback.Enabled = false;
                cmdCancel.Enabled = true;
                Cursor.Current = Cursors.WaitCursor;
                queryWorker.RunWorkerAsync(new SqlRunParameters(connStringBuilder, getSqlToRun(), cmbResultsType.SelectedItem.ToString()));
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

        private static void renderSchemaAndData(FlexResultSet resultSet, StringBuilder sb)
        {
            for (int i = 0; i < resultSet.results.Count; i++)
            {
                if (resultSet.results[i].recordsAffected > 0)
                {
                    sb.AppendLine(String.Format("--Records affected: {0:G}\r\n\r\n", resultSet.results[i].recordsAffected));
                }
                
                sb.AppendLine(resultSet.ScriptResultAsCreateTable(i, "#Result" + (i + 1).ToString()));
                sb.Append("\r\n");

                if (FieldScripting.ResultIsRenderableAsScriptedData(resultSet.results[i]))
                {
                    sb.AppendLine(FieldScripting.ScriptResultDataAsInsert(resultSet.results[i], "#Result" + (i + 1).ToString(), FlexResultSet.SQL2008MaxRowsInValuesClause).ToString());
                }
                
                sb.Append("\r\n");
            }
        }

        private static void renderExceptions(FlexResultSet resultSet, StringBuilder sb)
        {
            if (resultSet.exceptions.Count > 0)
            {
                sb.Append(String.Format("--There were {0} exception(s) encountered while running the query.\r\n", resultSet.exceptions.Count));
            }
            for (int i = 0; i < resultSet.exceptions.Count; i++)
            {
                var ex = resultSet.exceptions[i];
                if (ex is SqlResultProcessingException)
                {
                    sb.Append(String.Format("--Error processing result: \"{0}\".\r\n", ex.Message));
                }
                else if (ex is SqlExecutionException)
                {
                    sb.Append(String.Format("--Error executing query: \"{0}\".\r\n", ex.Message));
                }
                else
                {
                    sb.Append(String.Format("--Error: \"{0}\".\r\n", ex.Message));
                }
            }
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
            DoSqlQueryWork(e,queryWorker);
        }

        private static void DoSqlQueryWork(System.ComponentModel.DoWorkEventArgs e, BackgroundWorker bw)
        {
            FlexResultSet resultSet;
            var srp = (SqlRunParameters)e.Argument;
            bw.ReportProgress(1,"Opening connection...");
            using (SqlConnection conn = new SqlConnection(srp.connStringBuilder.ConnectionString))
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                conn.Open();
                bw.ReportProgress(2, "Running query...");

                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                resultSet = FlexResultSet.AnalyzeResultWithRollback(conn, srp.sqlToRun, bw);
                conn.Close();
            }
            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            bw.ReportProgress(90, "Scripting results...");
            var sb = new StringBuilder();
            srp.scriptedResult = sb;
            renderExceptions(resultSet, sb);

            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            bw.ReportProgress(92, "Scripting results...");
            if (srp.outputType == SqlRunParameters.TO_INSERT_STATEMENTS)
            {
                renderSchemaAndData(resultSet, sb);
            }
            else if (srp.outputType == SqlRunParameters.TO_XML_SPREADSHEET)
            {
                XmlSpreadsheetRenderer.renderAsXMLSpreadsheet(resultSet, srp.tempOutputStream);
            }

            e.Result = srp;
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
            lblProgress.Text = getFormattedElapsedSqlStopwatchTime(isComplete) + " " + progressText;
            lblProgress.Refresh();
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
                txtOutput.Text = "--Error occurred while processing query.\r\n\r\n/* " + e.Error.Message + "\r\n*/";
            }
            else
            {
                progressText = "Complete.";
                if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_INSERT_STATEMENTS)
                {
                    txtOutput.Text = srp.scriptedResult.ToString();
                }
                else if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_XML_SPREADSHEET)
                {
                    //todo: move this writing functionality to the core and out of the UI.
                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TSqlFlex" + DateTime.Now.ToString("_yyyyMMddTHHmmss") + ".xml");
                    File.Move(srp.tempOutputFileName, fileName);
                    txtOutput.Text = "--Results written to \"" + fileName + "\".\r\n\r\n--You can open this file in Excel.";
                }
                
            }
            
            setProgressText(true);

            lblConnectionInfo.Text = currentConnectionText();
            Cursor.Current = Cursors.Default;
            cmdCancel.Enabled = false;
            cmdRunNRollback.Enabled = true;
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

    }
}