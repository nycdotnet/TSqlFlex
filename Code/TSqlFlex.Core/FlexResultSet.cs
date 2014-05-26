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
        public List<DataTable> schemaTables = null;
        public List<Exception> exceptions = null;
        public List<FlexResult> results = null;
        
        public FlexResultSet() {
            schemaTables = new List<DataTable>();
            exceptions = new List<Exception>();
            results = new List<FlexResult>();        
        }

        public static FlexResultSet AnalyzeResultWithRollback(SqlConnection openConnection, string sqlCommandText) {

            FlexResultSet resultSet = new FlexResultSet();

            if (openConnection.State != System.Data.ConnectionState.Open)
            {
                var emptySqlConn = new ArgumentException("The SqlConnection must be open.");
                resultSet.exceptions.Add(emptySqlConn);
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
                        //todo: what if first SQL command has an exception - will the second one run/return results?
                        var st = reader.GetSchemaTable();
                        resultSet.schemaTables.Add(st);
                        FlexResult thisResultSet = new FlexResult();
                        int fieldCount = reader.FieldCount;
                        while (reader.Read())
                        {
                            Object[] values = new Object[fieldCount];
                            reader.GetValues(values);
                            thisResultSet.Add(values);
                        }
                        resultSet.results.Add(thisResultSet);

                    } while (reader.NextResult());
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                resultSet.exceptions.Add(ex);
            }
            finally
            {
                if (transaction != null)
                    transaction.Rollback();
            }
            return resultSet;
        }

        //todo: Support this query:
        /*  
         SELECT CAST(1 AS BIGINT) [aBigInt], CAST(1.0 AS NUMERIC(1,1)) AS [aNumeric], CAST(1 AS BIT) AS [aBit],
         * CAST(1 AS SMALLINT) AS [aSmallInt], CAST(1.0 AS DECIMAL(1,1)) AS [aDecimal], CAST(1.0 AS SMALLMONEY) as [aSmallMoney],
         * CAST(1 AS INT) as [anInt], CAST(1 as TINYINT) as [aTinyInt], CAST(1 as MONEY) as [aMoney], 
         * CAST(1 as FLOAT(53)) as [aFloat], CAST(1 as REAL),
         * CAST('2000-01-01T00:00:00' as DATE) as [aDate], CAST('2000-01-01T00:00:00+01:00:00' as DATETIMEOFFSET) as [aDateTimeOffset],
         * CAST('2000-01-01T00:00:00' as DATETIME2) as [aDateTime2], CAST('2000-01-01T00:00:00' as SMALLDATETIME) as [aSmallDateTime],
         * CAST('2000-01-01T00:00:00' as DATETIME) as [aDateTime], CAST('2000-01-01T00:00:00' as TIME) as [aTime],
         * CAST('ABC' as CHAR(3)) as [aCHAR], CAST('ABC' as VARCHAR(3)) as [aVarChar], CAST('ABC' as TEXT) as [aText],
         * CAST(N'ABC' as NCHAR(3)) as [anNCHAR], CAST(N'ABC' as NVARCHAR(3)) as [anNVarCHAR], CAST(N'ABC' as NTEXT) as [anNText],
         * CAST(1 as BINARY) as [aBinary], CAST(1 as VARBINARY) as [aVarBinary], CAST(1 as IMAGE) as [anImage],
         * CAST(0 AS TIMESTAMP) as [aTimestamp], CAST('/' as HIERARCHYID) as [aHierarchyID], NewID() as [aUniqueIdentifier],
         * CAST(1 as sql_variant), cast('<a />' as XML),
         *  (geography::STGeomFromText('LINESTRING(-122.360 47.656, -122.343 47.656 )', 4326)) as [aGeography],
         *  (geometry::STGeomFromText('LINESTRING (100 100, 20 180, 180 180)', 0)) as [aGeometry]
         * */


        public string ScriptResultAsCreateTable(int resultIndex, string tableName)
        {
            StringBuilder buffer = new StringBuilder("CREATE TABLE " + tableName + "(\r\n");
            var rows = schemaTables[resultIndex].Rows;
            for (int fieldIndex = 0; fieldIndex < rows.Count; fieldIndex++)
            {
                var fieldInfo = rows[fieldIndex];
                buffer.Append("    " +                           //indentation
                        FieldNameOrDefault(fieldInfo, fieldIndex) +         //field name
                        " " +
                        fieldInfo[24].ToString() +        //data type
                        DataTypeParameterIfAny(fieldInfo) + 
                        " " +
                        NullOrNotNull(fieldInfo[13])      //nullability
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

        private string FieldNameOrDefault(DataRow fieldInfo, int fieldIndex)
        {
            var r = fieldInfo[0].ToString();
            if (r.Length == 0)
            {
                return "anonymousColumn" + (fieldIndex + 1).ToString();
            }
            return r;
        }

        private string DataTypeParameterIfAny(DataRow fieldInfo)
        {
            var dataTypeName = fieldInfo[24].ToString();
            if (dataTypeName == "nvarchar" || dataTypeName == "varchar" || dataTypeName == "nchar" || dataTypeName == "char")
            {
                int columnSize = (int)fieldInfo[2];    
                if (columnSize == Int32.MaxValue)
                {
                    return "(MAX)";
                }
                return "(" + columnSize + ")";
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
