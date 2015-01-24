using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public static class QueryWorker
    {
        public static void DoSqlQueryWork(System.ComponentModel.DoWorkEventArgs e, BackgroundWorker bw)
        {
            FlexResultSet resultSet = null;
            var srp = (SqlRunParameters)e.Argument;

            bw.ReportProgress(1, "Opening connection...");
            SqlConnection conn = null;
            string currentTask = "";

            try
            {
                using (conn = new SqlConnection(srp.connStringBuilder.ConnectionString))
                {
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    currentTask = "while opening SQL connection";
                    conn.Open();
                    
                    bw.ReportProgress(2, "Running query...");

                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    currentTask = "while running the query or analyzing the data";
                    resultSet = FlexResultSet.AnalyzeResultWithRollback(conn, srp, bw);
                    
                    currentTask = "while closing the database connection";
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                renderExceptionToSqlRunParameters(currentTask, srp, ex);
            }
            finally
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            
            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            if (resultSet == null)
            {
                e.Result = srp;
                return;
            }

            try
            {
                bw.ReportProgress(90, "Scripting results...");
                renderAndCountExceptions(resultSet, srp);
            }
            catch (Exception ex)
            {
                renderExceptionToSqlRunParameters("scripting results", srp, ex);
                e.Result = srp;
                return;
            }


            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            bw.ReportProgress(92, "Scripting results...");
            if (srp.outputType == SqlRunParameters.TO_INSERT_STATEMENTS)
            {
                try
                {
                    renderSchemaAndData(resultSet, srp);
                }
                catch (Exception ex)
                {
                    renderExceptionToSqlRunParameters("while rendering schema and dataset", srp, ex);
                }
                
            }
            else if (srp.outputType == SqlRunParameters.TO_XML_SPREADSHEET)
            {
                try
                {
                    XmlSpreadsheetRenderer.renderAsXMLSpreadsheet(resultSet, srp);
                }
                catch (Exception ex)
                {
                    srp.worksheetIsValid = false;
                    srp.flushAndCloseOutputStreamIfNeeded();
                    renderExceptionToSqlRunParameters("while rendering spreadsheet", srp, ex);
                }
            }
            e.Result = srp;
        }

        private static void renderExceptionToSqlRunParameters(string generalDescriptionOfWhenTheErrorOccurred, SqlRunParameters srp, Exception ex)
        {
            srp.exceptionsText.Append("--Exception encountered ");
            srp.exceptionsText.Append(generalDescriptionOfWhenTheErrorOccurred);
            srp.exceptionsText.Append(".\r\n\r\n/* ");
            srp.exceptionsText.Append(ex.Message);
            srp.exceptionsText.Append("\r\n\r\n");
            srp.exceptionsText.Append(ex.StackTrace);
            srp.exceptionsText.Append("\r\n*/");
        }

        private static void renderAndCountExceptions(FlexResultSet resultSet, SqlRunParameters srp)
        {
            var sb = srp.exceptionsText;

            if (resultSet == null)
            {
                srp.exceptionCount = 0;
            }
            else
            {
                srp.exceptionCount = resultSet.exceptions.Count;
            }

            if (srp.exceptionCount > 0)
            {
                sb.Append(String.Format("--There were {0} exception(s) encountered while running the query.\r\n", resultSet.exceptions.Count));
            }
            for (int i = 0; i < srp.exceptionCount; i++)
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

        private static void renderSchemaAndData(FlexResultSet resultSet, SqlRunParameters srp)
        {
            var sb = srp.resultsText;
            for (int i = 0; i < resultSet.results.Count; i++)
            {
                if (resultSet.results[i].recordsAffected > 0)
                {
                    sb.AppendLine(String.Format("--Records affected: {0:G}\r\n\r\n", resultSet.results[i].recordsAffected));
                }
                string resultTableName = "#Result" + (i + 1 + srp.completedResultsCount).ToString();
                sb.AppendLine(resultSet.ScriptResultAsCreateTable(i, resultTableName));
                sb.Append("\r\n");

                if (FieldScripting.ResultIsRenderableAsScriptedData(resultSet.results[i]))
                {
                    sb.AppendLine(FieldScripting.ScriptResultDataAsInsert(resultSet.results[i], resultTableName, FlexResultSet.SQL2008MaxRowsInValuesClause).ToString());
                }

                srp.completedResultsCount += 1;

                sb.Append("\r\n");
            }
        }
    }
}
