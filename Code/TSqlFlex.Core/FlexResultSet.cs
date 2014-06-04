using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
                    buffer.Append(valueAsTSQLLiteral(data[rowIndex][columnIndex], schema.Rows[columnIndex]));
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

        private string valueAsTSQLLiteral(object data, DataRow fieldInfo)
        {
            if (data == null)
            {
                return "NULL"; //todo: this may or may not be accurate - need to check to see if nulls are preserved in this way.
            }
            var fieldTypeName = fieldInfo[DATA_TYPE_FIELD_INDEX].ToString();

            if (fieldTypeName == "char" || fieldTypeName == "varchar")
            {
                return "'" + data.ToString() + "'";
            }
            else if (fieldTypeName == "nchar" || fieldTypeName == "nvarchar")
            {
                return "N'" + data.ToString() + "'";
            }

            return "'" + data.ToString() + "'";
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
