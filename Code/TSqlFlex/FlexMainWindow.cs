using System;
using System.ComponentModel;
using System.Data.SqlClient;
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

        public FlexMainWindow()
        {
            InitializeComponent();
            lblVersion.Text = TSqlFlex.Core.Info.Version();
        }
    
        public void SetConnection(SqlConnectionStringBuilder theConnectionStringBuilder)
        {
            connStringBuilder = theConnectionStringBuilder;
            SetConnectionText();
        }

        private void SetConnectionText()
        {
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
                cmdRunNRollback.Enabled = false;
                cmdCancel.Enabled = true;
                Cursor.Current = Cursors.WaitCursor;
                queryWorker.RunWorkerAsync(new SqlRunParameters(connStringBuilder, getSqlToRun()));
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

                if (resultSet.ResultIsRenderableAsScriptedData(i))
                {
                    sb.AppendLine(resultSet.ScriptResultDataAsInsert(i, "#Result" + (i + 1).ToString()));
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
            if (txtOutput.Text.Length > 0)
            {
                Clipboard.SetText(txtOutput.Text);
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

                FlexResultSet resultSet = FlexResultSet.AnalyzeResultWithRollback(conn, srp.sqlToRun, bw);

                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                bw.ReportProgress(90, "Rendering results...");
                var sb = new StringBuilder();
                renderExceptions(resultSet, sb);

                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                bw.ReportProgress(95, "Rendering results...");
                renderSchemaAndData(resultSet, sb);

                e.Result = sb.ToString();
            }
        }

        private void queryWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e != null)
            {
                if (e.UserState is string && e.UserState != null)
                {
                    lblConnectionInfo.Text = currentConnectionText() + " " + (string)e.UserState;
                }

                queryProgress.Value = e.ProgressPercentage;
            }
        }

        private void queryWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            queryProgress.Value = 100;
            if (e.Cancelled)
            {
                txtOutput.Text = "--Query cancelled.";
            }
            else if (e.Error != null)
            {
                txtOutput.Text = "--Error occurred while processing query.\r\n\r\n/* " + e.Error.Message + "\r\n*/";
            }
            else
            {
                txtOutput.Text = (string)e.Result;
            }

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

    }
}