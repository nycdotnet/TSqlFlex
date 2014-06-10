using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;

namespace TSqlFlex.Core
{
    public class FlexResultSet
    {
        const int DATA_TYPE_FIELD_INDEX = 24;

        public List<FlexResult> results = null;
        
        public FlexResultSet() {
            results = new List<FlexResult>();        
        }

        public static FlexResultSet AnalyzeResultWithRollback(SqlConnection openConnection, string sqlCommandText) {

            FlexResultSet resultSet = new FlexResultSet();

            if (openConnection.State != System.Data.ConnectionState.Open)
            {
                var emptySqlConn = new ArgumentException("The SqlConnection must be open.");
                throw emptySqlConn;
            }
            
            SqlTransaction transaction = openConnection.BeginTransaction("Tran");
            
            try
            {
                SqlCommand cmd = new SqlCommand(sqlCommandText, openConnection, transaction);

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
                {
                    do
                    {
                        FlexResult result = new FlexResult();

                        try
                        {
                            var st = reader.GetSchemaTable();
                            result.schema = st;
                        }
                        catch (Exception ex)
                        {
                            result.exceptions.Add(ex);
                        }

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


                        resultSet.results.Add(result);

                    } while (reader.NextResult());
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (transaction != null)
                    transaction.Rollback();
            }
            return resultSet;
        }

        //todo: columnnames must be unique in a table.  It's possible to have a result set with duplicate column names, but not a table.

        public string ScriptResultAsCreateTable(int resultIndex, string tableName)
        {
            //todo: bug with SELECT * FROM INFORMATION_SCHEMA.Tables - possibly hidden fields??
            if (results[resultIndex].schema == null)
            {
                return "--No schema for result from query.";
            }
            var rows = results[resultIndex].schema.Rows;
            StringBuilder buffer = new StringBuilder("CREATE TABLE " + tableName + "(\r\n");
            for (int fieldIndex = 0; fieldIndex < rows.Count; fieldIndex++)
            {
                var fieldInfo = rows[fieldIndex];
                buffer.Append("    " +
                        FieldNameOrDefault(fieldInfo, fieldIndex) +
                        " " +
                        DataType(fieldInfo) +
                        DataTypeParameterIfAny(fieldInfo) + 
                        " " +
                        NullOrNotNull(fieldInfo[13])
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

        public string ScriptResultDataAsInsert100(int resultIndex, string tableName)
        {
            var schema = results[resultIndex].schema;

            if (schema == null)
            {
                return "--No schema for result from query.";
            }

            var data = results[resultIndex].data;

            if (data == null)
            {
                return "--No data for result from query.";
            }

            int columnCount = schema.Rows.Count;
            int rowCount = data.Count;

            StringBuilder buffer = new StringBuilder();

            buffer.Append("INSERT INTO " + tableName + " VALUES\r\n");

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                buffer.Append("  (");
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    buffer.Append(valueAsTSQLLiteral(data[rowIndex][columnIndex], schema.Rows[columnIndex].ItemArray));
                    if (columnIndex + 1 < columnCount)
                    {
                        buffer.Append(",");
                    }
                }
                if (rowIndex + 1 == rowCount)
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
            if (data == null)
            {
                return "NULL"; //todo: this may or may not be accurate - need to check to see if nulls are always presented this way in an ADO.NET reader.
            }
            var fieldTypeName = fieldInfo[DATA_TYPE_FIELD_INDEX].ToString();

            if (fieldTypeName == "char")
            {
                return "'" + data.ToString().TrimEnd() + "'";
            }
            else if (fieldTypeName == "varchar" || fieldTypeName == "text")
            {
                return "'" + data.ToString() + "'";
            }
            else if (fieldTypeName == "nchar")
            {
                return "N'" + data.ToString().TrimEnd() + "'";
            }
            else if (fieldTypeName == "nvarchar" || fieldTypeName == "ntext" || fieldTypeName == "xml")
            {
                return "N'" + data.ToString() + "'";
            }
            else if (fieldTypeName == "bigint" || fieldTypeName == "numeric" || fieldTypeName == "smallint" || fieldTypeName == "decimal" || fieldTypeName == "smallmoney" ||
                fieldTypeName == "int" || fieldTypeName == "tinyint" || fieldTypeName == "float" || fieldTypeName == "real" || fieldTypeName == "money")
            {
                return data.ToString();
            }
            else if (fieldTypeName == "binary" || fieldTypeName == "rowversion" || fieldTypeName == "timestamp")
            {
                byte[] ba = (byte[])data;
                string bitsAsHexString = BitConverter.ToString(ba).Replace("-", "");
                int charCountToShowAsHex = (int)fieldInfo[2] * 2;
                bitsAsHexString = bitsAsHexString.PadLeft(charCountToShowAsHex,'0');
                return "0x" + bitsAsHexString;
            }
            else if (fieldTypeName == "date") {
                DateTime d = (DateTime)data;
                return "'" + d.ToString("yyyy-MM-dd") + "'";
            }
            else if (fieldTypeName == "datetimeoffset")
            {
                DateTimeOffset d = (DateTimeOffset)data;
                if (d.ToString("fffffff") == "0000000")
                {
                    return "'" + d.ToString("yyyy-MM-ddTHH:mm:sszzzz") + "'";
                }
                return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzzz") + "'";
            }
            else if (fieldTypeName == "datetime2")
            {
                DateTime d = (DateTime)data;
                if (d.ToString("fffffff") == "0000000")
                {
                    return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
                }
                return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss.fffffff") + "'";
            }
            else if (fieldTypeName == "time")
            {
                DateTime d = (DateTime)data;
                if (d.ToString("fffffff") == "0000000")
                {
                    return "'" + d.ToString("HH:mm:ss") + "'";
                }
                return "'" + d.ToString("HH:mm:ss.fffffff") + "'";
            }
            else if (fieldTypeName == "datetime")
            {
                DateTime d = (DateTime)data;
                if (d.ToString("fff") == "000")
                {
                    return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
                }
                return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "'";
            }
            else if (fieldTypeName == "smalldatetime")
            {
                DateTime d = (DateTime)data;
                return "'" + d.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
            }
            else if (fieldTypeName == "bit")
            {
                if ((bool)data == true)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            else if (fieldTypeName == "varbinary" || fieldTypeName == "image")
            {
                byte[] ba = (byte[])data;
                return "0x" + BitConverter.ToString(ba).Replace("-", "");
            }
            else if (fieldTypeName == "uniqueidentifier")
            {
                Guid g = (Guid)data;
                return "'" + g.ToString("D").ToUpper() + "'";
            }
            else if (fieldTypeName == "sql_variant")
            {
                //todo: this is a placeholder.  Need to refactor each of the serializations and then figure out how best to call them appropriately.
                return "'" + data.ToString() + "'";

            }
            else if (fieldTypeName.EndsWith("hierarchyid"))
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
            else if (fieldTypeName.EndsWith("geography"))
            {
                SqlGeography geog = (SqlGeography)data;
                return "N'" + geog.STAsText().ToSqlString().ToString() + "'";
            }
            else if (fieldTypeName.EndsWith("geometry"))
            {
                SqlGeometry geom = (SqlGeometry)data;
                return "N'" + geom.STAsText().ToSqlString().ToString() + "'";
            }
            //shouldn't get here.  In-place for future data type compatibility.
            return "N'" + data.ToString() + "'";
        }



        private string FieldNameOrDefault(DataRow fieldInfo, int fieldIndex)
        {
            var r = fieldInfo[0].ToString();
            if (r.Length == 0)
            {
                return "anonymousColumn" + (fieldIndex + 1).ToString();
            }
            return r;
        }

        private string DataType(DataRow fieldInfo)
        {
            var fieldTypeName = fieldInfo[DATA_TYPE_FIELD_INDEX].ToString();
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
            var dataTypeName = fieldInfo[DATA_TYPE_FIELD_INDEX].ToString();
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
