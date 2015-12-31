using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SqlServer.Types;
using System.ComponentModel;
using System.Diagnostics;

namespace TSqlFlex.Core
{
    public class FlexResultSet
    {

        public const int SQL2008MaxRowsInValuesClause = 100;

        public List<FlexResult> results = null;
        public List<Exception> exceptions = null;
        
        public FlexResultSet() {
            results = new List<FlexResult>();
            exceptions = new List<Exception>();
        }

        public static FlexResultSet AnalyzeResultWithRollback(SqlConnection openConnection, SqlRunParameters srp, BackgroundWorker bw = null)
        {

            FlexResultSet resultSet = new FlexResultSet();

            throwExceptionIfConnectionIsNotOpen(openConnection);
            
            SqlTransaction transaction = openConnection.BeginTransaction("Tran");
            SqlDataReader reader = null;

            try
            {
                SqlCommand cmd = new SqlCommand(srp.sqlToRun, openConnection, transaction);
                
                //todo: this is a bad way of doing this.  Need to abstract further.
                bw.ReportProgress(5, "Running query...");

                reader = executeSQL(resultSet, cmd, reader);
                
                int progress = 50;
                bw.ReportProgress(progress, "Processing results...");
                do
                {
                    FlexResult result = new FlexResult();
                    if (reader != null)
                    {
                        try
                        {
                            result.recordsAffected = reader.RecordsAffected;
                            processSchemaInfo(reader, result);
                            if (progress < 90)
                            {
                                progress += 5;
                            }
                            bw.ReportProgress(progress, "Processing results...");
                            processData(reader, result);

                            if (progress < 90)
                            {
                                progress += 5;
                            }
                            bw.ReportProgress(progress, "Processing results...");
                        }
                        catch (Exception ex)
                        {
                            resultSet.exceptions.Add(new SqlResultProcessingException(ex));
                        }
                    }
                    foreach (Exception ex in result.exceptions)
                    {
                        resultSet.exceptions.Add(ex);
                    }
                    resultSet.results.Add(result);

                } while (reader != null && reader.NextResult());
            }
            catch (Exception ex)
            {
                resultSet.exceptions.Add(new SqlResultProcessingException(ex));
            }
            finally
            {
                cleanupReader(reader);
                rollbackTransaction(transaction);
            }
            return resultSet;
        }

        private static void rollbackTransaction(SqlTransaction transaction)
        {
            if (transaction != null)
            { 
                transaction.Rollback();
            }
        }

        private static void cleanupReader(SqlDataReader reader)
        {
            if (reader != null)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                reader.Dispose();
            }
        }

        private static SqlDataReader executeSQL(FlexResultSet resultSet, SqlCommand cmd, SqlDataReader reader)
        {
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
            }
            catch (Exception ex)
            {
                resultSet.exceptions.Add(new SqlExecutionException(ex));
            }
            return reader;
        }

        private static void throwExceptionIfConnectionIsNotOpen(SqlConnection openConnection)
        {
            if (openConnection.State != System.Data.ConnectionState.Open)
            {
                var emptySqlConn = new ArgumentException("The SqlConnection must be open.");
                throw emptySqlConn;
            }
        }

        private static void processData(SqlDataReader reader, FlexResult result)
        {
            try
            {
                int fieldCount = reader.FieldCount;
                var data = new List<Object[]>();
                while (reader.Read())
                {
                    Object[] values = new Object[fieldCount];
                    reader.GetValues(values);
                    data.Add(values);
                }
                result.data = data;
            }
            catch (Exception ex)
            {
                result.exceptions.Add(ex);
            }
        }

        private static void processSchemaInfo(SqlDataReader reader, FlexResult result)
        {
            try
            {
                result.schema = SQLColumnList.CreateFromSchemaTable(reader.GetSchemaTable());
            }
            catch (Exception ex)
            {
                result.exceptions.Add(ex);
            }
        }

        public string ScriptResultAsCreateTable(int resultIndex, string tableName)
        {
            //todo: columnnames must be unique in a table.  It's possible to have a result set with duplicate column names, but not a table.
            //todo: bug with SELECT * FROM INFORMATION_SCHEMA.Tables - possibly hidden fields??
            if (!FieldScripting.ResultIsRenderableAsCreateTable(results[resultIndex]))
            {
                return "--No schema for result from query.";
            }

            int visibleColumnCount = results[resultIndex].visibleColumnCount;
            var rows = results[resultIndex].schema;
            StringBuilder buffer = new StringBuilder("CREATE TABLE " + tableName + "(\r\n");
            for (int fieldIndex = 0; fieldIndex < results[resultIndex].visibleColumnCount; fieldIndex++)
            {
                var fieldInfo = rows[fieldIndex];
                buffer.Append("    " +
                        FieldScripting.FieldNameOrDefault(fieldInfo, fieldIndex) +
                        " " +
                        FieldScripting.DataTypeName(fieldInfo) +
                        FieldScripting.DataTypeParameterIfAny(fieldInfo) + 
                        " " +
                        FieldScripting.NullOrNotNull(fieldInfo.AllowNulls)
                        );

                if (fieldIndex + 1 < visibleColumnCount)
                {
                    buffer.Append(",\r\n");
                } else {
                    buffer.Append("\r\n");
                }
            }
            buffer.Append(");\r\n");
            return buffer.ToString();
        }

    }
}
