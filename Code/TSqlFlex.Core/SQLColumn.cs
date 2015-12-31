using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class SQLColumn
    {
        public string ColumnName { get; set; }
        public string BaseTableName { get; set; }
        public int ColumnSize { get; set; }
        public short NumericPrecision { get; set; }
        public short NumericScale { get; set; }
        public string DataType { get; set; }
        public bool AllowNulls { get; set; }
        public bool IsHidden { get; set; }
    }

    public static class SQLColumnList
    {
        public static List<SQLColumn> CreateFromSchemaTable(DataTable table)
        {
            var result = new List<SQLColumn>(table.Columns.Count);
            foreach (DataRow fieldDefinition in table.Rows)
            {
                result.Add(new SQLColumn()
                {
                    ColumnName = fieldDefinition[(int)FieldScripting.ADONetFieldInfo.Name].ToString(),
                    BaseTableName = fieldDefinition[11].ToString(),
                    ColumnSize = (int)fieldDefinition[(int)FieldScripting.ADONetFieldInfo.ColumnSize],
                    NumericPrecision = (short)fieldDefinition[(int)FieldScripting.ADONetFieldInfo.NumericPrecision],
                    NumericScale = (short)fieldDefinition[(int)FieldScripting.ADONetFieldInfo.NumericScale],
                    DataType = fieldDefinition[(int)FieldScripting.ADONetFieldInfo.DataType].ToString(),
                    AllowNulls = (bool)fieldDefinition[(int)FieldScripting.ADONetFieldInfo.AllowsNulls],
                    IsHidden = fieldDefinition[(int)FieldScripting.ADONetFieldInfo.IsHidden] == DBNull.Value ? false : (bool)fieldDefinition[(int)FieldScripting.ADONetFieldInfo.IsHidden] 
                });
            }
            return result;
        }
    }

}
