using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SqlServer.Types;
using System.ComponentModel;

namespace TSqlFlex.Core
{
    public class FlexResultSet
    {
        enum FieldInfo : int
        {
            Name = 0,
            FieldLength = 2,
            AllowsNulls = 13,
            DataType = 24
        }

        public const int SQL2008MaxRowsInValuesClause = 100;

        public List<FlexResult> results = null;
        public List<Exception> exceptions = null;
        
        public FlexResultSet() {
            results = new List<FlexResult>();
            exceptions = new List<Exception>();
        }

        public static FlexResultSet AnalyzeResultWithRollback(SqlConnection openConnection, string sqlCommandText, BackgroundWorker bw = null)
        {

            FlexResultSet resultSet = new FlexResultSet();

            throwExceptionIfConnectionIsNotOpen(openConnection);
            
            SqlTransaction transaction = openConnection.BeginTransaction("Tran");
            SqlDataReader reader = null;

            try
            {
                SqlCommand cmd = new SqlCommand(sqlCommandText, openConnection, transaction);
                
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
                var st = reader.GetSchemaTable();
                result.schema = st;
            }
            catch (Exception ex)
            {
                result.exceptions.Add(ex);
            }
        }

        public Boolean ResultIsRenderableAsCreateTable(int resultIndex)
        {
            return (results[resultIndex].schema != null);
        }
        
        public string ScriptResultAsCreateTable(int resultIndex, string tableName)
        {
            //todo: columnnames must be unique in a table.  It's possible to have a result set with duplicate column names, but not a table.
            //todo: bug with SELECT * FROM INFORMATION_SCHEMA.Tables - possibly hidden fields??
            if (!ResultIsRenderableAsCreateTable(resultIndex))
            {
                return "--No schema for result from query.";
            }
            var rows = results[resultIndex].schema.Rows;
            StringBuilder buffer = new StringBuilder("CREATE TABLE " + tableName + "(\r\n");
            for (int fieldIndex = 0; fieldIndex < rows.Count; fieldIndex++)
            {
                var fieldInfo = rows[fieldIndex];
                buffer.Append("    " +
                        FieldNameOrDefault(fieldInfo.ItemArray, fieldIndex) +
                        " " +
                        DataType(fieldInfo) +
                        DataTypeParameterIfAny(fieldInfo) + 
                        " " +
                        NullOrNotNull(fieldInfo.ItemArray[(int)FieldInfo.AllowsNulls])
                        );
                if (fieldIndex + 1 < rows.Count)
                {
                    buffer.Append(",\r\n");
                } else {
                    buffer.Append("\r\n");
                }
            }
            buffer.Append(");\r\n");
            return buffer.ToString();
        }

        public Boolean ResultIsRenderableAsScriptedData(int resultIndex)
        {
            var r = results[resultIndex];
            return (r.schema != null && r.data != null && r.data.Count > 0);
        }

        public string ScriptResultDataAsInsert(int resultIndex, string tableName)
        {
            if (!ResultIsRenderableAsCreateTable(resultIndex))
            {
                return "--No schema for result from query.";
            }
            
            if (!ResultIsRenderableAsScriptedData(resultIndex))
            {
                return "--No rows were returned from the query.";
            }

            var schema = results[resultIndex].schema;
            var data = results[resultIndex].data;
            return scriptDataAsInsertForSQL2008Plus(tableName, schema, data);
        }

        private static string scriptDataAsInsertForSQL2008Plus(string tableName, DataTable schema, List<object[]> data)
        {
            int columnCount = schema.Rows.Count;
            int rowCount = data.Count;

            StringBuilder buffer = new StringBuilder();

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                if (rowIndex % SQL2008MaxRowsInValuesClause == 0)
                {
                    buffer.Append("INSERT INTO " + tableName + " VALUES\r\n");
                }
                buffer.Append(" (");
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    buffer.Append(valueAsTSQLLiteral(data[rowIndex][columnIndex], schema.Rows[columnIndex].ItemArray));
                    if (columnIndex + 1 < columnCount)
                    {
                        buffer.Append(",");
                    }
                }
                if (rowIndex + 1 == rowCount || (rowIndex + 1) % SQL2008MaxRowsInValuesClause == 0)
                {
                    buffer.Append(");\r\n\r\n");
                }
                else
                {
                    buffer.Append("),\r\n");
                }
            }

            return buffer.ToString();
        }

        //todo: may need some refactoring :-)
        public static string valueAsTSQLLiteral(object data, object[] fieldInfo)
        {
            if (data == null || data is DBNull)
            {
                return "NULL";
            }

            var fieldTypeName = fieldInfo[(int)FieldInfo.DataType].ToString();

            if (fieldTypeName == "char")
            {
                return getDataAsCharFormat(data);
            }
            else if (fieldTypeName == "varchar" || fieldTypeName == "text")
            {
                return getDataAsVarcharFormat(data);
            }
            else if (fieldTypeName == "nchar")
            {
                return getDataAsNcharFormat(data);
            }
            else if (fieldTypeName == "nvarchar" || fieldTypeName == "ntext" || fieldTypeName == "xml")
            {
                return getDataAsNvarcharFormat(data);
            }
            else if (fieldTypeName == "bigint" || fieldTypeName == "numeric" || fieldTypeName == "smallint" || fieldTypeName == "decimal" || fieldTypeName == "smallmoney" ||
                fieldTypeName == "int" || fieldTypeName == "tinyint" || fieldTypeName == "float" || fieldTypeName == "real" || fieldTypeName == "money")
            {
                return getDataAsAppropriateNumericFormat(data);
            }
            else if (fieldTypeName == "binary" || fieldTypeName == "rowversion" || fieldTypeName == "timestamp")
            {
                return getDataAsBinaryFormat(data, fieldInfo);
            }
            else if (fieldTypeName == "date") {
                return getDataAsDateFormat(data);
            }
            else if (fieldTypeName == "datetimeoffset")
            {
                return getDataAsDatetimeoffsetFormat(data);
            }
            else if (fieldTypeName == "datetime2")
            {
                return getDataAsDatetime2Format(data);
            }
            else if (fieldTypeName == "time")
            {
                return getDataAsTimeFormat(data);
            }
            else if (fieldTypeName == "datetime")
            {
                return getDataAsDatetimeFormat(data);
            }
            else if (fieldTypeName == "smalldatetime")
            {
                return getDataAsSmalldatetimeFormat(data);
            }
            else if (fieldTypeName == "bit")
            {
                return getDataAsBitFormat(data);
            }
            else if (fieldTypeName == "varbinary" || fieldTypeName == "image")
            {
                return getDataAsVarbinaryFormat(data);
            }
            else if (fieldTypeName == "uniqueidentifier")
            {
                return getDataAsGuidFormat(data);
            }
            else if (fieldTypeName == "sql_variant")
            {
                return getDataAsSql_variantFormat(data);
            }
            else if (fieldTypeName.EndsWith("hierarchyid"))
            {
                return getDataAsHierarchyIdFormat(data);
            }
            else if (fieldTypeName.EndsWith("geography"))
            {
                return getDataAsGeographyFormat(data);
            }
            else if (fieldTypeName.EndsWith("geometry"))
            {
                return getDataAsGeometryFormat(data);
            }
            //shouldn't get here.  In-place for future data type compatibility.
            if (data is string)
            {
                return "N'" + ((string)data).Replace("'","''") + "'";
            }
            return "N'" + data.ToString() + "'";
        }

        private static string getDataAsAppropriateNumericFormat(object data)
        {
            if (data is decimal)
            {
                decimal theDec = (decimal)data;
                if (partAfterDecimal(theDec) == 0)
                {
                    return theDec.ToString("F0");
                }
                return theDec.ToString("G").TrimEnd('0');
            }
            else if (data is Double)
            {
                Double theDbl = (Double)data;
                if (partAfterDecimal(theDbl) == 0)
                {
                    return theDbl.ToString("F0");
                }
                return theDbl.ToString("F7").TrimEnd('0');
            }
            else if (data is Single)
            {
                Single theSingle = (Single)data;
                if (partAfterDecimal(theSingle) == 0)
                {
                    return theSingle.ToString("F0");
                }
                return theSingle.ToString("F7").TrimEnd('0');
            }
            
            return data.ToString();
        }

        private static double partAfterDecimal(Single theSingle)
        {
            return theSingle - Math.Truncate(theSingle);
        }

        private static double partAfterDecimal(Double theDbl)
        {
            return theDbl - Math.Truncate(theDbl);
        }

        private static decimal partAfterDecimal(decimal theDec)
        {
            return theDec - Math.Truncate(theDec);
        }

        private static string getDataAsSql_variantFormat(object data)
        {
            //SQL-CLR Type Mapping documentation: http://msdn.microsoft.com/en-us/library/bb386947(v=vs.110).aspx

            if (data is SqlGeometry)
            {
                return getDataAsGeometryFormat(data);
            }
            else if (data is SqlGeography)
            {
                return getDataAsGeographyFormat(data);
            }
            else if (data is SqlHierarchyId)
            {
                return getDataAsHierarchyIdFormat(data);
            }
            else if (data is Guid)
            {
                return getDataAsGuidFormat(data);
            }
            else if (data is byte[])
            {
                return getDataAsVarbinaryFormat(data);
            }
            else if (data is DateTimeOffset)
            {
                return getDataAsDatetimeoffsetFormat(data);
            }
            else if (data is DateTime)
            {
                return getDataAsDatetimeFormat(data);
            }
            else if (data is TimeSpan)
            {
                return getDataAsTimeFormat(data);
            }
            else if (data is bool)
            {
                return getDataAsBitFormat(data);
            }
            else if (data is decimal || data is Double || data is Single)
            {
                return getDataAsAppropriateNumericFormat(data);
            }
            else if (data is string)
            {
                return "N'" + data.ToString().Replace("'","''") + "'";
            }

            //All numeric types
            return data.ToString();
            
        }

        private static string getDataAsGeometryFormat(object data)
        {
            SqlGeometry geom = (SqlGeometry)data;
            return "geometry::STGeomFromText('" + geom.STAsText().ToSqlString().ToString() + "'," + geom.STSrid.ToSqlString().ToString() + ")";
        }

        private static string getDataAsGeographyFormat(object data)
        {
            SqlGeography geog = (SqlGeography)data;
            return "geography::STGeomFromText('" + geog.STAsText().ToSqlString().ToString() + "'," + geog.STSrid.ToSqlString().ToString() + ")";
        }

        private static string getDataAsHierarchyIdFormat(object data)
        {
            SqlHierarchyId hier = (SqlHierarchyId)data;
            byte[] ba;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(ms))
            {
                hier.Write(w);
                w.Flush();
                ba = ms.ToArray();
            }
            return "0x" + BitConverter.ToString(ba).Replace("-", "");
        }

        private static string getDataAsGuidFormat(object data)
        {
            Guid g = (Guid)data;
            return "'" + g.ToString("D").ToUpper() + "'";
        }

        private static string getDataAsVarbinaryFormat(object data)
        {
            byte[] ba = (byte[])data;
            return "0x" + BitConverter.ToString(ba).Replace("-", "");
        }

        private static string getDataAsBitFormat(object data)
        {
            if ((bool)data == true)
            {
                return "1";
            }
            return "0";
        }

        private static string getDataAsSmalldatetimeFormat(object data)
        {
            DateTime d = (DateTime)data;
            if (d.Hour == 0 && d.Minute == 0) //smalldatetime doesn't support seconds.
            {
                return "'" + d.ToString("yyyy-MM-dd") + "'";
            }
            return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
        }

        private static string getDataAsDatetimeFormat(object data)
        {
            DateTime d = (DateTime)data;
            if (d.ToString("fff") == "000")
            {
                if (d.Hour == 0 && d.Minute == 0 & d.Second == 0)
                {
                    return "'" + d.ToString("yyyy-MM-dd") + "'";
                }
                return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
            }
            return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss.fff").TrimEnd('0') + "'";
        }


        private static string getDataAsTimeFormat(object data)
        {
            if (data is TimeSpan)
            {
                TimeSpan t = (TimeSpan)data;
                if (t.Milliseconds == 0)
                {
                    return String.Format("'{0}:{1}:{2}'",
                        t.Hours.ToString().PadLeft(2, '0'),
                        t.Minutes.ToString().PadLeft(2, '0'),
                        t.Seconds.ToString().PadLeft(2, '0'));
                }
                return String.Format("'{0}:{1}:{2}.{3}'",
                        t.Hours.ToString().PadLeft(2, '0'),
                        t.Minutes.ToString().PadLeft(2, '0'),
                        t.Seconds.ToString().PadLeft(2, '0'),
                        t.Milliseconds.ToString().PadLeft(3, '0').TrimEnd('0'));
            }
            else if (data is DateTime)
            {
                DateTime d = (DateTime)data;

                if (d.ToString("fffffff") == "0000000")
                {
                    return "'" + d.ToString("HH:mm:ss") + "'";
                }
                return "'" + d.ToString("HH:mm:ss.fffffff").TrimEnd('0') + "'";
            }
            
            return "'" + data.ToString() + "'"; //should not get hit.
        }

        private static string getDataAsDatetime2Format(object data)
        {
            DateTime d = (DateTime)data;

            if (d.ToString("fffffff") == "0000000")
            {
                if (d.Hour == 0 && d.Minute == 0 & d.Second == 0)
                {
                    return "'" + d.ToString("yyyy-MM-dd") + "'";
                }
                return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
            }
            return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss.fffffff").TrimEnd('0') + "'";
        }

        private static string getDataAsDatetimeoffsetFormat(object data)
        {
            DateTimeOffset d = (DateTimeOffset)data;
            if (d.ToString("fffffff") == "0000000")
            {
                return "'" + d.ToString("yyyy-MM-ddTHH:mm:sszzzz") + "'";
            }
            return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzzz") + "'";
        }

        private static string getDataAsDateFormat(object data)
        {
            DateTime d = (DateTime)data;
            return "'" + d.ToString("yyyy-MM-dd") + "'";
        }

        private static string getDataAsBinaryFormat(object data, object[] fieldInfo)
        {
            byte[] ba = (byte[])data;
            string bitsAsHexString = BitConverter.ToString(ba).Replace("-", "");
            int charCountToShowAsHex = (int)fieldInfo[(int)FieldInfo.FieldLength] * 2;
            bitsAsHexString = bitsAsHexString.PadLeft(charCountToShowAsHex, '0');
            return "0x" + bitsAsHexString;
        }

        private static string getDataAsNvarcharFormat(object data)
        {
            return "N'" + data.ToString().Replace("'","''") + "'";
        }

        private static string getDataAsNcharFormat(object data)
        {
            return "N'" + data.ToString().Replace("'", "''").TrimEnd() + "'";
        }

        private static string getDataAsVarcharFormat(object data)
        {
            return "'" + data.ToString().Replace("'", "''") + "'";
        }

        private static string getDataAsCharFormat(object data)
        {
            return "'" + data.ToString().Replace("'", "''").TrimEnd() + "'";
        }

        public static string FieldNameOrDefault(object[] fieldInfo, int fieldIndex)
        {
            var r = fieldInfo[(int)FieldInfo.Name].ToString();
            if (r.Length == 0)
            {
                return "anonymousColumn" + (fieldIndex + 1).ToString();
            }
            if (TSqlRules.IsReservedWord(r))
            {
                return "[" + r + "]";
            }
            return r; //bug: possibly need to escape [ or ] in field names?
        }

        private string DataType(DataRow fieldInfo)
        {
            var fieldTypeName = fieldInfo[(int)FieldInfo.DataType].ToString();
            if (fieldTypeName == "real")
            {
                return "float";  //this could be a float or a real.  There is no simple way to tell via ado.net.  Will try to keep it consistent with float.
            }
            else if (fieldTypeName.EndsWith(".sys.hierarchyid"))
            {
                return "hierarchyid";
            }
            else if (fieldTypeName.EndsWith(".sys.geography"))
            {
                return "geography";
            }
            else if (fieldTypeName.EndsWith(".sys.geometry"))
            {
                return "geometry"; 
            }
            return fieldTypeName;
        }

        private string DataTypeParameterIfAny(DataRow fieldInfo)
        {
            var dataTypeName = fieldInfo[(int)FieldInfo.DataType].ToString();
            if (dataTypeName == "nvarchar" || dataTypeName == "varchar" || dataTypeName == "nchar" || dataTypeName == "char" || dataTypeName == "binary" || dataTypeName == "varbinary")
            {
                int columnSize = (int)fieldInfo[2];
                if (columnSize == Int32.MaxValue)
                {
                    return "(MAX)";
                }
                return "(" + columnSize.ToString() + ")";
            }
            else if (dataTypeName == "numeric" || dataTypeName == "decimal")
            {
                int numericPrecision = (short)fieldInfo[3];
                int numericScale = (short)fieldInfo[4];
                return "(" + numericPrecision.ToString() + "," + numericScale.ToString() + ")";
            }
            else if (dataTypeName == "real")
            {
                return "(24)";
            }
            else if (dataTypeName == "float")
            {
                //from MSDN: SQL Server treats n as one of two possible values. If 1<=n<=24, n is treated as 24. If 25<=n<=53, n is treated as 53.
                return "(53)";
            }
            else if (dataTypeName == "datetimeoffset" || dataTypeName == "time")
            {
                int numericPrecision = (short)fieldInfo[4];
                //see: http://msdn.microsoft.com/en-us/library/bb630289.aspx
                
                if (numericPrecision <= 2 )
                {
                    return "(2)";
                }
                if (numericPrecision <=4)
                {
                    return "(4)";
                }
                return "";
            }
            return "";
        }

        public string NullOrNotNull(Object allowDbNull)
        {
            bool allowDBNullFlag;
            if (bool.TryParse(allowDbNull.ToString(), out allowDBNullFlag))
            {
                if (allowDBNullFlag)
                {
                    return "NULL";
                }
                return "NOT NULL";
            }
            return "NULL"; //safer default for our purposes.  This is unlikely to be hit anyway.
        }
    }
}
