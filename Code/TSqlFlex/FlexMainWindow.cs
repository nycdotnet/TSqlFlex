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

        private static void renderAsXMLSpreadsheet(FlexResultSet resultSet, StringBuilder sb)
        {
            //todo: refactor this and FlexResultSet to to share code and have test coverage.
            sb.Append(Utils.GetResourceByName("TSqlFlex.Core.Resources.XMLSpreadsheetTemplateHeader.txt"));
            for (int i = 0; i < resultSet.results.Count; i++)
            {
                var result = resultSet.results[i];
                int columnCount = result.schema.Rows.Count; //you find the column count by counting the rows in the schema.

                sb.Append(String.Format("<Worksheet ss:Name=\"Sheet{0}\">", i + 1));
                sb.Append(String.Format("<Table ss:ExpandedColumnCount=\"{0}\" ss:ExpandedRowCount=\"{1}\" x:FullColumns=\"1\" x:FullRows=\"1\" ss:DefaultRowHeight=\"15\">",
                    columnCount,
                    result.data.Count + 1 /* include header row */)
                    );

                //do header
                sb.Append("<Row>");
                for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                {
                    sb.Append(String.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", escapeForXML((string)result.schema.Rows[colIndex].ItemArray[(int)TSqlFlex.Core.FlexResultSet.FieldInfo.Name])));
                }
                sb.Append("</Row>");

                //do data rows
                for (int rowIndex = 0; rowIndex < result.data.Count; rowIndex += 1)
                {
                    sb.Append("<Row>");
                    for (int colIndex = 0; colIndex < columnCount; colIndex += 1) {
                        object fieldData = result.data[rowIndex][colIndex];
                        string fieldTypeName = result.schema.Rows[colIndex].ItemArray[(int)TSqlFlex.Core.FlexResultSet.FieldInfo.DataType].ToString();
                        if (fieldData == null || fieldData is DBNull)
                        {
                            sb.Append("<Cell/>");
                        }
                        else if (fieldTypeName == "bigint" || fieldTypeName == "numeric" || fieldTypeName == "smallint" || fieldTypeName == "decimal" || fieldTypeName == "smallmoney" ||
                            fieldTypeName == "int" || fieldTypeName == "tinyint" || fieldTypeName == "float" || fieldTypeName == "real" || fieldTypeName == "money")
                        {
                            sb.Append(String.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>\r\n", escapeForXML(fieldData.ToString())));
                        }
                        else if (fieldTypeName == "date" || fieldTypeName == "datetime2" || fieldTypeName == "time" || fieldTypeName == "datetime" ||
                            fieldTypeName == "smalldatetime")
                        {
                            sb.Append(String.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"DateTime\">{0}</Data></Cell>\r\n", escapeForXML(
                                ((DateTime)fieldData).ToString("yyyy-MM-ddTHH:mm:ss.fff")
                                )));
                        }
                        else
                        {
                            sb.Append(String.Format("<Cell ss:StyleID=\"s64\"><Data ss:Type=\"String\">{0}</Data></Cell>\r\n", escapeForXML(result.data[rowIndex][colIndex].ToString())));
                        }
                        
                    }
                    sb.Append("</Row>");
                }

                sb.Append("</Table></Worksheet>");
            }
            sb.Append("</Workbook>\r\n");
        }

        private static string escapeForXML(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"","&quot;").Replace("'","&apos;");
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
                    sb.AppendLine(resultSet.ScriptResultDataAsInsert(i, "#Result" + (i + 1).ToString()).ToString());
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
                renderAsXMLSpreadsheet(resultSet, sb);
            }

            e.Result = sb.ToString();
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
                    txtOutput.Text = (string)e.Result;
                }
                else if (cmbResultsType.SelectedItem.ToString() == SqlRunParameters.TO_XML_SPREADSHEET)
                {
                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TSqlFlex" + DateTime.Now.ToString("_yyyyMMddTHHmmss") + ".xml");
                    using (StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Create), Encoding.UTF8))
                    {
                        sw.WriteLine((string)e.Result);
                        sw.Flush();
                        sw.Close();
                    }
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