using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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

        public static string GetResourceByName(string resourceName)
        {
            string result;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
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
