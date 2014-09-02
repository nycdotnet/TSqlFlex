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
                        srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", escapeForXML((string)result.schema.Rows[colIndex].ItemArray[(int)FieldScripting.FieldInfo.Name])));
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
                            object[] fieldInfo = result.schema.Rows[colIndex].ItemArray;
                            string fieldTypeName = fieldInfo[(int)FieldScripting.FieldInfo.DataType].ToString();
                            if (fieldData == null || fieldData is DBNull)
                            {
                                srp.WriteToStream("<Cell/>");
                            }
                            else if (fieldTypeName == "bigint" || fieldTypeName == "numeric" || fieldTypeName == "smallint" || fieldTypeName == "decimal" || fieldTypeName == "smallmoney" ||
                                fieldTypeName == "int" || fieldTypeName == "tinyint" || fieldTypeName == "float" || fieldTypeName == "real" || fieldTypeName == "money" || fieldTypeName == "bit")
                            {
                                srp.WriteToStream(String.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>\r\n", escapeForXML(FieldScripting.valueAsTSQLLiteral(fieldData, fieldInfo, false))));
                            }
                            else if (fieldTypeName == "date" || fieldTypeName == "datetime2" || fieldTypeName == "time" || fieldTypeName == "datetime" ||
                                fieldTypeName == "smalldatetime")
                            {
                                srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"DateTime\">{0}.{1}</Data></Cell>\r\n",
                                    escapeForXML(((DateTime)fieldData).ToString("s")),
                                    escapeForXML(((DateTime)fieldData).ToString("fff"))
                                    ));
                            }
                            else if (fieldTypeName == "binary" || fieldTypeName == "rowversion" || fieldTypeName == "timestamp")
                            {
                                byte[] d = (byte[])result.data[rowIndex][colIndex];
                                srp.WriteToStream(String.Format("<Cell ss:StyleID=\"s64\"><Data ss:Type=\"String\">{0}</Data></Cell>\r\n", escapeForXML(FieldScripting.formatBinary(d,d.Length))));
                            }
                            else if (fieldTypeName == "varbinary" || fieldTypeName == "image")
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

        private static string escapeForXML(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
