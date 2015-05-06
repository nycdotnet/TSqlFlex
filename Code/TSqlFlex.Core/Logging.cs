using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace TSqlFlex.Core
{
    public class Logging
    {
        public bool verboseLogging { get; set; }
        public bool logToDebugger { get; set; }

        public Exception lastException = null;

        private StreamWriter outputLogStream;
        private const string SubKey = "Software\\LegendaryApps.com\\T-SQL Flex";

        public Logging()
        {
            verboseLogging = false;
            logToDebugger = false;
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
                if (!String.IsNullOrEmpty(lfn) && lastException == null){
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
        

        private bool localMachineDWORDEquals1(string subKey, string value)
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(subKey);
                if (key == null)
                {
                    return false;
                }

                object data = key.GetValue(value);
                if (data == null)
                {
                    return false;
                }

                Int32 intData = Convert.ToInt32(data);

                return (intData == 1);
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }


        private bool logToDebuggerRegistryValue()
        {
            return localMachineDWORDEquals1(SubKey, "LogToDebugger");
        }

        private bool verboseLoggingRegistryValue()
        {
            return localMachineDWORDEquals1(SubKey, "VerboseLogging");
        }

        public string logFileNameRegistryValue()
        {
            try
            {
                var legendaryAppsKey = Registry.LocalMachine.OpenSubKey(SubKey);
                if (legendaryAppsKey == null)
                {
                    return "";
                }

                object logFileName = legendaryAppsKey.GetValue("LogFileName");
                if (logFileName == null || !(logFileName is string)) {
                    return "";
                }

                string strLogFileName = logFileName.ToString();

                if (string.IsNullOrEmpty(strLogFileName))
                {
                    return "";
                }

                return strLogFileName;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return "";
            }
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
