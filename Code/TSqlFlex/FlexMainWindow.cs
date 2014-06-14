using System;
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
            if (Utils.IsValidConnectionStringBuilder(connStringBuilder))
            {
                lblConnectionInfo.Text = "Instance: " + connStringBuilder.DataSource + ", DB: " + connStringBuilder.InitialCatalog;
            } else {
                lblConnectionInfo.Text = "Not connected.";
            }
        }

        private void cmdRunNRollback_Click(object sender, EventArgs e)
        {
            if (!Utils.IsValidConnectionStringBuilder(connStringBuilder))
            {
                MessageBox.Show("Select the target database in the object tree first.");
                return;
            }
            
            using (SqlConnection conn = new SqlConnection(connStringBuilder.ConnectionString))
            {
                conn.Open();

                FlexResultSet resultSet = FlexResultSet.AnalyzeResultWithRollback(conn, getSqlToRun());

                var sb = new StringBuilder();

                renderExceptions(resultSet, sb);
                renderSchemaAndData(resultSet, sb);

                txtOutput.Text = sb.ToString();
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

    }
}