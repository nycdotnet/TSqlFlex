using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            //todo: check for a valid connection here.
            //todo: refactor so all of the code is not behind the button.
            StringBuilder buffer = new StringBuilder("");
            DataTable schemaTable;
            SqlTransaction transaction = null;
            using (SqlConnection conn = new SqlConnection(connString.ConnectionString))
            {
                conn.Open();
                transaction = conn.BeginTransaction("Tran");
                try
                {
                    SqlCommand cmd = new SqlCommand(txtSqlInput.Text, conn, transaction);

                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        schemaTable = reader.GetSchemaTable();

                        //See http://support.microsoft.com/kb/310107
                        if (schemaTable != null)
                        {  //todo: handle SQL messages such as row counts, etc, if possible.
                            foreach (DataRow myField in schemaTable.Rows)
                            {
                                foreach (DataColumn myProperty in schemaTable.Columns)
                                {
                                    if (myField[myProperty].ToString().Length > 0)
                                    {
                                        buffer.Append("--" + myProperty.ColumnName + " = " + myField[myProperty].ToString() + "\r\n");
                                    }
                                }
                            }
                        }

                        while (reader.Read())
                        {
                            for (int fieldIndex = 0; fieldIndex < reader.FieldCount; fieldIndex++)
                            {
                                buffer.Append(reader[fieldIndex] + ",");
                            }
                            buffer.Append("\r\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    buffer.Append("\r\n\r\nError selecting from the DB:" + ex.Message + "\n" + ex.StackTrace);
                }
                finally
                {
                    if (transaction != null)
                        transaction.Rollback();
                }
                txtOutput.Text = buffer.ToString();
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