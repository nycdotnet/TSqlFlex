using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

    public class Info
    {
        public static string Version() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return "v" + fvi.FileVersion + "-alpha";
        }
    }

}
