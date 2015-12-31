using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class XmlSpreadsheetRenderer
    {
        public static void renderAsXMLSpreadsheet(FlexResultSet resultSet, SqlRunParameters srp)
        {
            //todo: refactor this and FlexResultSet to to share code and have test coverage.
            srp.WriteToStream(Utils.GetResourceByName("TSqlFlex.Core.Resources.XMLSpreadsheetTemplateHeader.txt"));
            for (int i = 0; i < resultSet.results.Count; i++)
            {
                FlexResult result = resultSet.results[i];
                if (result.schema != null && result.data != null)
                {
                    int columnCount = result.visibleColumnCount;

                    srp.WriteToStream(String.Format("<Worksheet ss:Name=\"Sheet{0}\">", i + 1));
                    srp.WriteToStream(String.Format("<Table ss:ExpandedColumnCount=\"{0}\" ss:ExpandedRowCount=\"{1}\" x:FullColumns=\"1\" x:FullRows=\"1\" ss:DefaultRowHeight=\"15\">",
                        columnCount,
                        result.data.Count + 1 /* include header row */)
                        );

                    //do header
                    srp.WriteToStream("<Row>");
                    for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                    {

                        srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", columnName(result, colIndex)));
                    }
                    srp.WriteToStream("</Row>\r\n");

                    //do data rows
                    for (int rowIndex = 0; rowIndex < result.data.Count; rowIndex += 1)
                    {
                        srp.WriteToStream("<Row>");
                        for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                        {
                            //todo: fix each of these items to work with the actual scripting stuff (requires finishing major refactoring work).
                            object fieldData = result.data[rowIndex][colIndex];
                            SQLColumn fieldInfo = result.schema[colIndex];
                            
                            if (fieldData == null || fieldData is DBNull)
                            {
                                srp.WriteToStream("<Cell/>");
                            }
                            else if (fieldInfo.DataType == "bigint" || fieldInfo.DataType == "numeric" || fieldInfo.DataType == "smallint" || fieldInfo.DataType == "decimal" || fieldInfo.DataType == "smallmoney" ||
                                fieldInfo.DataType == "int" || fieldInfo.DataType == "tinyint" || fieldInfo.DataType == "float" || fieldInfo.DataType == "real" || fieldInfo.DataType == "money" || fieldInfo.DataType == "bit")
                            {
                                srp.WriteToStream(String.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>\r\n", escapeForXML(FieldScripting.valueAsTSQLLiteral(fieldData, fieldInfo, false))));
                            }
                            else if (fieldInfo.DataType == "date" || fieldInfo.DataType == "datetime2" || fieldInfo.DataType == "time" || fieldInfo.DataType == "datetime" ||
                                fieldInfo.DataType == "smalldatetime")
                            {
                                srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"DateTime\">{0}.{1}</Data></Cell>\r\n",
                                    escapeForXML(((DateTime)fieldData).ToString("s")),
                                    escapeForXML(((DateTime)fieldData).ToString("fff"))
                                    ));
                            }
                            else if (fieldInfo.DataType == "binary" || fieldInfo.DataType == "rowversion" || fieldInfo.DataType == "timestamp")
                            {
                                byte[] d = (byte[])result.data[rowIndex][colIndex];
                                srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s64\"><Data ss:Type=\"String\">{0}</Data></Cell>\r\n", escapeForXML(FieldScripting.formatBinary(d,d.Length))));
                            }
                            else if (fieldInfo.DataType == "varbinary" || fieldInfo.DataType == "image")
                            {
                                srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s64\"><Data ss:Type=\"String\">{0}</Data></Cell>\r\n", escapeForXML(FieldScripting.formatVarbinary(fieldData))));
                            }
                            else
                            {
                                srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s64\"><Data ss:Type=\"String\">{0}</Data></Cell>\r\n", escapeForXML(FieldScripting.valueAsTSQLLiteral(fieldData, fieldInfo, false))));
                            }

                        }
                        srp.WriteToStream("</Row>\r\n");
                    }

                    srp.WriteToStream("</Table></Worksheet>\r\n");
                    srp.worksheetIsValid = true;
                }
            }
            srp.WriteToStream("</Workbook>\r\n");
        }

        private static string columnName(FlexResult result, int zeroBasedColumnIndex)
        {
            if (String.IsNullOrEmpty(result.schema[zeroBasedColumnIndex].ColumnName))
            {
                return "anonymousColumn" + (zeroBasedColumnIndex + 1).ToString();
            }
            return escapeForXML(result.schema[zeroBasedColumnIndex].ColumnName);
        }

        private static string escapeForXML(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
