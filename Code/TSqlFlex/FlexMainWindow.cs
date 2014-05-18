using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TSqlFlex.Core;

namespace TSqlFlex
{
    public partial class FlexMainWindow : UserControl
    {

        private SqlConnectionStringBuilder connString = null;

        public FlexMainWindow()
        {
            InitializeComponent();
        }
    
        public void SetConnection(SqlConnectionStringBuilder theConnectionString)
        {
            connString = theConnectionString;
            SetConnectionText();
        }

        private void SetConnectionText()
        {
            if (connString != null && connString.DataSource.Length > 0 && connString.InitialCatalog.Length > 0)
            {
                lblConnectionInfo.Text = "Instance: " + connString.DataSource + ", DB: " + connString.InitialCatalog;
            } else {
                lblConnectionInfo.Text = "Not connected.";
            }
        }

        private void cmdRunNRollback_Click(object sender, EventArgs e)
        {
            //bug: check for a valid connection here.  Will throw exception if DB is not selected.
            using (SqlConnection conn = new SqlConnection(connString.ConnectionString))
            {
                conn.Open();

                FlexResultSet resultSet = FlexResultSet.AnalyzeResultWithRollback(conn, txtSqlInput.Text);

                txtOutput.Text = resultSet.ScriptResultAsCreateTable(0, "#MyNewTable");

                //txtOutput.Text = buffer.ToString();
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