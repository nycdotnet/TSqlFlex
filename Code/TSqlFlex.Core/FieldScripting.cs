using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public static class FieldScripting
    {
        public enum FieldInfo : int
        {
            Name = 0,
            FieldLength = 2,
            AllowsNulls = 13,
            DataType = 24
        }

        public static string DataTypeName(DataRow fieldInfo)
        {
            var fieldTypeName = fieldInfo[(int)FieldInfo.DataType].ToString();
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

        public static string DataTypeParameterIfAny(DataRow fieldInfo)
        {
            var dataTypeName = fieldInfo[(int)FieldInfo.DataType].ToString();
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

                if (numericPrecision <= 2)
                {
                    return "(2)";
                }
                if (numericPrecision <= 4)
                {
                    return "(4)";
                }
                return "";
            }
            return "";
        }

        //todo: try to eliminate the .ToString() in here.
        public static string NullOrNotNull(Object allowDbNull)
        {
            if (allowDbNull is bool)
            {
                if ((bool)allowDbNull)
                {
                    return "NULL";
                }
                return "NOT NULL";
            }
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

        public static string FieldNameOrDefault(object[] fieldInfo, int fieldIndex)
        {
            var r = fieldInfo[(int)FieldScripting.FieldInfo.Name].ToString();
            if (r.Length == 0)
            {
                return "anonymousColumn" + (fieldIndex + 1).ToString();
            }
            if (TSqlRules.IsReservedWord(r))
            {
                return "[" + r + "]";
            }
            return r; //bug: possibly need to escape [ or ] in field names?
        }
    }
}
