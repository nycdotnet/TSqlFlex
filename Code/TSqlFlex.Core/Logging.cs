using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class Logging
    {
        public bool verboseLogging { get; set; }
        public bool logToDebugger { get; set; }
        public Config config;

        public Exception lastException = null;

        private StreamWriter outputLogStream;

        public Logging(Config instantiatedConfig)
        {
            config = instantiatedConfig;
            initializeLogging();
        }

        ~Logging()
        {
            if (isLogging())
            {
                outputLogStream.Flush();
                outputLogStream.Close();
            }
        }

        private void initializeLogging()
        {
            try
            {
                logToDebugger = logToDebuggerRegistryValue();
                verboseLogging = verboseLoggingRegistryValue();
                lastException = null;
                var lfn = logFileNameRegistryValue();
                if (!String.IsNullOrEmpty(lfn)){
                    Stream logStream = new FileStream(lfn, FileMode.Append, FileAccess.Write, FileShare.Read);
                    outputLogStream = new StreamWriter(logStream, Encoding.UTF8);
                    LogVerbose("Opened outputLogStream at " + lfn + ".");
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Exception trying to set up log.  " + ex.Message);
            }

        }
        

        private bool logToDebuggerRegistryValue()
        {
            return config.LogToDebugger;
        }

        private bool verboseLoggingRegistryValue()
        {
            return config.VerboseLogging;
        }

        public string logFileNameRegistryValue()
        {
            return config.LogFileName;
        }

        public void LogVerbose(string TextToLog)
        {
            if (verboseLogging)
            {
                Log(TextToLog);
            }
        }

        public bool isLogging()
        {
            try
            {
                if (outputLogStream == null || outputLogStream.BaseStream == null)
                {
                    return false;
                }
                return (outputLogStream.BaseStream.CanWrite);
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        public void Log(string TextToLog)
        {
            var logging = isLogging();
            if (logging || logToDebugger)
            {
                string logEntry = "\"" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffffff") + "\",\"" + TextToLog.Replace("\"","\"\"") + "\"";
                if (logging)
                {
                    try
                    {
                        outputLogStream.WriteLine(logEntry);
                        outputLogStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        Debug.Print("Exception trying to log: " + ex.Message);
                        Debug.Print("log entry was: " + logEntry);
                    }

                }
                if (logToDebugger)
                {
                    Debug.Print(logEntry);
                }
            }
        }
    }
}
