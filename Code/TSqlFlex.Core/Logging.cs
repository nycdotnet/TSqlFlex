using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public static class Logging
    {
        public static bool verboseLogging { get; set; }

        static Logging()
        {
            verboseLogging = true;
        }

        public static void Log(string TextToLog, bool VerboseModeOnly = false)
        {
            if ((VerboseModeOnly && verboseLogging) || VerboseModeOnly == false) {
                Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffffff") + " " + TextToLog);
            }
        }
    }
}
