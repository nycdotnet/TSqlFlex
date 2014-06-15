using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TSqlFlex
{
    //todo: this belongs in TSqlFlex.Core.
    class SqlRunParameters
    {
        public SqlConnectionStringBuilder connStringBuilder;
        public string sqlToRun;
        public SqlRunParameters(SqlConnectionStringBuilder csb, string sqlToRun)
        {
            this.connStringBuilder = csb;
            this.sqlToRun = sqlToRun;
        }
    }
}
