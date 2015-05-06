using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class CSVRenderer
    {
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
                            object[] fieldInfo = result.schema.Rows[colIndex].ItemArray;
                            string fieldTypeName = fieldInfo[(int)FieldScripting.FieldInfo.DataType].ToString();
                            if (fieldData == null || fieldData is DBNull)
                            {
                                //do nothing
                            }
                            else if (fieldTypeName == "bigint" || fieldTypeName == "numeric" || fieldTypeName == "smallint" || fieldTypeName == "decimal" || fieldTypeName == "smallmoney" ||
                                fieldTypeName == "int" || fieldTypeName == "tinyint" || fieldTypeName == "float" || fieldTypeName == "real" || fieldTypeName == "money" || fieldTypeName == "bit")
                            {
                                //todo: may be bug in German culture where they use , as the decimal separator.
                                srp.WriteToStream(FieldScripting.valueAsTSQLLiteral(fieldData, fieldInfo, false));
                            }
                            else if (fieldTypeName == "date" || fieldTypeName == "datetime2" || fieldTypeName == "time" || fieldTypeName == "datetime" ||
                                fieldTypeName == "smalldatetime")
                            {
                                srp.WriteToStream(escapeForCSV(String.Format("{0}.{1}",
                                    ((DateTime)fieldData).ToString("s"),
                                    ((DateTime)fieldData).ToString("fff")
                                    )));
                            }
                            else if (fieldTypeName == "binary" || fieldTypeName == "rowversion" || fieldTypeName == "timestamp")
                            {
                                byte[] d = (byte[])result.data[rowIndex][colIndex];
                                srp.WriteToStream(escapeForCSV(FieldScripting.formatBinary(d, d.Length)));
                            }
                            else if (fieldTypeName == "varbinary" || fieldTypeName == "image")
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
            string headerName = (string)result.schema.Rows[zeroBasedColumnIndex].ItemArray[(int)FieldScripting.FieldInfo.Name];
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
