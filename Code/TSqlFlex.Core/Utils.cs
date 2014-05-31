using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class Utils
    {
        public static bool IsValidConnectionStringBuilder(SqlConnectionStringBuilder builder)
        {
            if (builder != null)
            {
                if (!string.IsNullOrEmpty(builder.DataSource) && !(string.IsNullOrEmpty(builder.InitialCatalog))) {
                    return (builder.IntegratedSecurity || !string.IsNullOrEmpty(builder.UserID)); //technically someone could have a blank password?
                }
            }
            return false;
        }
    }
}
