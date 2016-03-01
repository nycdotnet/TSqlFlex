using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class CSharpRenderer
    {

        public static void renderAsCSharp(FlexResultSet resultSet, SqlRunParameters srp)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < resultSet.results.Count; i++)
            {
                FlexResult result = resultSet.results[i];
                if (result.schema != null && result.data != null)
                {
                    int columnCount = result.visibleColumnCount;
                    var columnNamesAndCounts = new Dictionary<string, int>();
                    sb.Append(String.Format(classHeader, generateValidCSharpClassName(i)));
                    for (int colIndex = 0; colIndex < columnCount; colIndex += 1)
                    {
                        var s = result.schema[colIndex];
                        sb.Append(DisambiguateAndRenderCSharpProperty(s, columnNamesAndCounts));
                    }
                    sb.Append(classFooter);
                }
            }
            srp.resultsText = sb;
        }

        private static string generateValidCSharpClassName(int i)
        {
            return "Result" + i.ToString();
        }

        private static Dictionary<string, string> SqlDataTypeToCSharp = new Dictionary<string, string>() {
            {"int","int"},
            {"smallint", "short"},
            {"bigint", "long"},
            {"varchar", "string"},
            {"text", "string"},
            {"nvarchar", "string"},
            {"ntext", "string"},
            {"char", "string"},
            {"nchar", "string"},
            {"xml", "string"},
            {"numeric", "decimal"},
            {"smallmoney", "decimal"},
            {"tinyint", "byte"},
            {"float", "double"},
            {"real", "single"},
            {"money", "decimal"},
            {"binary", "byte[]"},
            {"varbinary", "byte[]"},
            {"image", "byte[]"},
            {"rowversion", "byte[]"},
            {"timestamp", "byte[]"},
            {"date", "DateTime"},
            {"datetimeoffset", "DateTimeOffset"},
            {"datetime", "DateTime"},
            {"datetime2", "DateTime"},
            {"time", "DateTime"},
            {"smalldatetime", "DateTime"},
            {"bit", "bool"},
            {"uniqueidentifier", "Guid"},
            {"sql_variant", "object"},
            {"hierarchyid", "object"},
            {"geography", "object"},
            {"geometry", "object"}
        };

        public static string DisambiguateAndRenderCSharpProperty(SQLColumn s, Dictionary<string, int> columnNamesAndCounts)
        {
            string name = CSharpRenderer.FieldNameToCSharpPropertyName(s.ColumnName);
            if (columnNamesAndCounts.ContainsKey(name))
            {
                columnNamesAndCounts[name] += 1;
                name = String.Format("{0}_{1}", name, columnNamesAndCounts[name]);
            }
            else
            {
                columnNamesAndCounts.Add(name, 1);
            }

            return RenderCSharpProperty(s, name);   
        }

        public static string RenderCSharpProperty(SQLColumn s, string name)
        {
            if (CSharpRenderer.SqlDataTypeToCSharp.ContainsKey(s.DataType))
            {
                return String.Format(propertyBoilerplate, CSharpRenderer.SqlDataTypeToCSharp[s.DataType], name);
            }
            else
            {
                return String.Format(propertyBoilerplate, "object", name);
            }
        }

        public static bool IsCSharpLetterCharacter(char value)
        {
            return char.IsLetter(value) ||
                CharUnicodeInfo.GetUnicodeCategory(value) == UnicodeCategory.LetterNumber;
        }

        public static bool IsCSharpCombiningConnectingOrFormattingCharacter(char value)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(value);
            return category == UnicodeCategory.NonSpacingMark ||
                category == UnicodeCategory.SpacingCombiningMark ||
                category == UnicodeCategory.ConnectorPunctuation ||
                category == UnicodeCategory.Format;
        }

        public static string FieldNameToCSharpPropertyName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return "anonymousProperty";
            }
            var fieldNameChars = fieldName.ToCharArray();
            for (int i = 0; i < fieldNameChars.Length; i++)
            {
                char current = fieldNameChars[i];
                if (i == 0)
                {
                    if (!IsCSharpLetterCharacter(current) && current != '@')
                    {
                        fieldNameChars[i] = '_';
                    }
                }
                else
                {
                    if (!IsCSharpLetterCharacter(current) &&
                        !char.IsDigit(current) &&
                        !IsCSharpCombiningConnectingOrFormattingCharacter(current)
                        )
                    {
                        fieldNameChars[i] = '_';
                    }
                }
            }
            return new string(fieldNameChars);

        }
        

        private const string classHeader = "public class {0}\r\n{{\r\n";
        private const string classFooter = "}\r\n";
        private const string propertyBoilerplate = "    public {0} {1} {{ get; set; }}\r\n";

    }
}
