using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class XmlSpreadsheetRenderer
    {
        public static void renderAsXMLSpreadsheet(FlexResultSet resultSet, StreamWriter sw)
        {
            //todo: refactor this and FlexResultSet to to share code and have test coverage.
            sw.Write(Utils.GetResourceByName("TSqlFlex.Core.Resources.XMLSpreadsheetTemplateHeader.txt"));
            for (int i = 0; i < resultSet.results.Count; i++)
            {
                FlexResult result = resultSet.results[i];
                int columnCount = result.visibleColumnCount;

                sw.Write(String.Format("<Worksheet ss:Name=\"Sheet{0}\">", i + 1));
                sw.Write(String.Format("<Table ss:ExpandedColumnCount=\"{0}\" ss:ExpandedRowCount=\"{1}\" x:FullColumns=\"1\" x:FullRows=\"1\" ss:DefaultRowHeight=\"15\">",
                    columnCount,
                    result.data.Count + 1 /* include header row */)
                    );

                //do header
                sw.Write("<Row>");
                for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                {
                    sw.Write(String.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", escapeForXML((string)result.schema.Rows[colIndex].ItemArray[(int)FieldScripting.FieldInfo.Name])));
                }
                sw.Write("</Row>");

                //do data rows
                for (int rowIndex = 0; rowIndex < result.data.Count; rowIndex += 1)
                {
                    sw.Write("<Row>");
                    for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                    {
                        object fieldData = result.data[rowIndex][colIndex];
                        string fieldTypeName = result.schema.Rows[colIndex].ItemArray[(int)FieldScripting.FieldInfo.DataType].ToString();
                        if (fieldData == null || fieldData is DBNull)
                        {
                            sw.Write("<Cell/>");
                        }
                        else if (fieldTypeName == "bigint" || fieldTypeName == "numeric" || fieldTypeName == "smallint" || fieldTypeName == "decimal" || fieldTypeName == "smallmoney" ||
                            fieldTypeName == "int" || fieldTypeName == "tinyint" || fieldTypeName == "float" || fieldTypeName == "real" || fieldTypeName == "money")
                        {
                            sw.Write(String.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>\r\n", escapeForXML(fieldData.ToString())));
                        }
                        else if (fieldTypeName == "date" || fieldTypeName == "datetime2" || fieldTypeName == "time" || fieldTypeName == "datetime" ||
                            fieldTypeName == "smalldatetime")
                        {
                            sw.Write(String.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"DateTime\">{0}</Data></Cell>\r\n", escapeForXML(
                                ((DateTime)fieldData).ToString("yyyy-MM-ddTHH:mm:ss.fff")
                                )));
                        }
                        else
                        {
                            sw.Write(String.Format("<Cell ss:StyleID=\"s64\"><Data ss:Type=\"String\">{0}</Data></Cell>\r\n", escapeForXML(result.data[rowIndex][colIndex].ToString())));
                        }

                    }
                    sw.Write("</Row>");
                }

                sw.Write("</Table></Worksheet>");
            }
            sw.Write("</Workbook>\r\n");
        }

        private static string escapeForXML(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
