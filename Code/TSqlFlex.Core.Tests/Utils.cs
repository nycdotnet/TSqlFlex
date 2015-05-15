using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TSqlFlex.Core.Tests
{
    internal class Utils
    {
        public static Stream GetResourceAsStream(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
