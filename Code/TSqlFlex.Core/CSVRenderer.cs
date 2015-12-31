using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class CSVRenderer
    {
        private static HashSet<string> numberLikeDataTypesForCSV = new HashSet<string>() { "bigint", "numeric", "smallint", "decimal", "smallmoney", "int", "tinyint", "float", "real", "money", "bit" };
        private static HashSet<string> dateLikeDataTypesForCSV = new HashSet<string>() { "date", "datetime2", "time", "datetime", "smalldatetime" }; 

        public static void renderAsCSV(FlexResultSet resultSet, SqlRunParameters srp)
        {
            int writtenResultSets = 0;
            for (int i = 0; i < resultSet.results.Count; i++)
            {
                FlexResult result = resultSet.results[i];
                if (result.schema != null && result.data != null)
                {
                    writtenResultSets += 1;
                    if (writtenResultSets > 1)
                    {
                        srp.openNewOutputStream();
                    }

                    int columnCount = result.visibleColumnCount;

                    //do header
                    for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                    {
                        srp.WriteToStream(columnName(result, colIndex));
                        if (colIndex + 1 < columnCount)
                        {
                            srp.WriteToStream(",");
                        }
                        else
                        {
                            srp.WriteToStream("\r\n");
                        }
                    }

                    //do data rows
                    for (int rowIndex = 0; rowIndex < result.data.Count; rowIndex += 1)
                    {
                        for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                        {
                            //todo: fix each of these items to work with the actual scripting stuff (requires finishing major refactoring work).
                            object fieldData = result.data[rowIndex][colIndex];
                            SQLColumn fieldInfo = result.schema[colIndex];
                            
                            if (fieldData == null || fieldData is DBNull)
                            {
                                //do nothing
                            }
                            else if (numberLikeDataTypesForCSV.Contains(fieldInfo.DataType))
                            {
                                //todo: may be bug in German culture where they use , as the decimal separator.
                                srp.WriteToStream(FieldScripting.valueAsTSQLLiteral(fieldData, fieldInfo, false));
                            }
                            else if (dateLikeDataTypesForCSV.Contains(fieldInfo.DataType))
                            {
                                srp.WriteToStream(escapeForCSV(String.Format("{0}.{1}",
                                    ((DateTime)fieldData).ToString("s"),
                                    ((DateTime)fieldData).ToString("fff")
                                    )));
                            }
                            else if (fieldInfo.DataType == "binary" || fieldInfo.DataType == "rowversion" || fieldInfo.DataType == "timestamp")
                            {
                                byte[] d = (byte[])result.data[rowIndex][colIndex];
                                srp.WriteToStream(escapeForCSV(FieldScripting.formatBinary(d, d.Length)));
                            }
                            else if (fieldInfo.DataType == "varbinary" || fieldInfo.DataType == "image")
                            {
                                srp.WriteToStream(escapeForCSV(FieldScripting.formatVarbinary(fieldData)));
                            }
                            else
                            {
                                srp.WriteToStream(escapeForCSV(FieldScripting.valueAsTSQLLiteral(fieldData, fieldInfo, false)));
                            }

                            if (colIndex + 1 < columnCount)
                            {
                                srp.WriteToStream(",");
                            }
                            else
                            {
                                srp.WriteToStream("\r\n");
                            };
                        }
                        
                    }

                    srp.worksheetIsValid = true;
                }
            }
        }

        private static string columnName(FlexResult result, int zeroBasedColumnIndex)
        {
            string headerName = (string)result.schema[zeroBasedColumnIndex].ColumnName;
            if (headerName == "")
            {
                return "anonymousColumn" + (zeroBasedColumnIndex + 1).ToString();
            }
            return escapeForCSV(headerName);
        }

        public static string escapeForCSV(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            if (input == "0")
            {
                return input;
            }
            if (input.Substring(0, 1) == "0" ||
                input.Contains('"') ||
                input.Contains(',') ||
                input.Contains('\n') ||
                input.Contains('\r'))
                
            {
                return "\"" + input.Replace("\"", "\"\"") + "\"";
            }
            return input;
        }


    }
}
