using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class SqlRunParameters
    {
        public const string TO_INSERT_STATEMENTS = "To INSERT Statements";
        public const string TO_XML_SPREADSHEET = "To XML Spreadsheet (Excel)";

        public SqlConnectionStringBuilder connStringBuilder;
        public string sqlToRun;
        public string outputType;
        public SqlRunParameters(SqlConnectionStringBuilder csb, string sqlToRun, string outputType)
        {
            this.connStringBuilder = csb;
            this.sqlToRun = sqlToRun;
            this.outputType = outputType;
        }
    }
}
