using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
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

        //attempt to open all user stores in order of preference.
        public static IsolatedStorageFile getIsolatedStorageFile()
        {
            IsolatedStorageFile isf = null;
            try
            {
                isf = IsolatedStorageFile.GetUserStoreForDomain();
            }
            catch (Exception)
            {
                isf = null;
            }
            
            if (isf == null)
            {
                try
                {
                    isf = IsolatedStorageFile.GetUserStoreForAssembly();
                }
                catch (Exception)
                {
                    isf = null;
                }
            }

            if (isf == null)
            {
                try
                {
                    isf = IsolatedStorageFile.GetUserStoreForApplication();
                }
                catch (Exception)
                {
                    //since this is the last one, give up and throw the exception.
                    throw;
                }
            }

            return isf;
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
