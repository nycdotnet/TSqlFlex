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
            FlexResultSet resultSet;
            var srp = (SqlRunParameters)e.Argument;
            bw.ReportProgress(1, "Opening connection...");
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

                resultSet = FlexResultSet.AnalyzeResultWithRollback(conn, srp, bw);
                conn.Close();
            }
            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            bw.ReportProgress(90, "Scripting results...");

            renderAndCountExceptions(resultSet, srp);

            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            bw.ReportProgress(92, "Scripting results...");
            if (srp.outputType == SqlRunParameters.TO_INSERT_STATEMENTS)
            {
                renderSchemaAndData(resultSet, srp);
            }
            else if (srp.outputType == SqlRunParameters.TO_XML_SPREADSHEET)
            {
                try
                {
                    XmlSpreadsheetRenderer.renderAsXMLSpreadsheet(resultSet, srp);
                }
                catch (Exception)
                {
                    srp.worksheetIsValid = false;
                    srp.flushAndCloseOutputStreamIfNeeded();
                }
                
            }

            e.Result = srp;
        }

        private static void renderAndCountExceptions(FlexResultSet resultSet, SqlRunParameters srp)
        {
            var sb = srp.exceptionsText;
            srp.exceptionCount = resultSet.exceptions.Count;

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

        private static void renderSchemaAndData(FlexResultSet resultSet, SqlRunParameters srp)
        {
            var sb = srp.resultsText;
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
    }
}
