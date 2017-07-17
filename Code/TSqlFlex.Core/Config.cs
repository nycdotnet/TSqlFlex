using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace TSqlFlex.Core
{
    public class Config
    {
        public int CommandTimeoutInSeconds { get; private set; }
        public bool LogToDebugger { get; private set; }
        public bool VerboseLogging { get; private set; }
        public string LogFileName { get; private set; }

        private const string TSqlFlexKey = "Software\\LegendaryApps.com\\T-SQL Flex";

        public Config()
        {
            CommandTimeoutInSeconds = getLocalMachineDWORD(TSqlFlexKey, "CommandTimeoutInSeconds").GetValueOrDefault(60 * 5);
            LogToDebugger = getLocalMachineDWORD(TSqlFlexKey, "LogToDebugger") == 1;
            VerboseLogging = getLocalMachineDWORD(TSqlFlexKey, "VerboseLogging") == 1;
            LogFileName = getLocalMachineREGSZ(TSqlFlexKey, "LogFileName") ?? "";
        }

        internal static string getLocalMachineREGSZ(string subKey, string value)
        {
            try
            {
                var rk = Registry.LocalMachine.OpenSubKey(subKey, RegistryKeyPermissionCheck.ReadSubTree);
                var result = rk?.GetValue(value) as string;
                rk?.Close();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static int? getLocalMachineDWORD(string subKey, string value)
        {
            try
            {
                var rk = Registry.LocalMachine.OpenSubKey(subKey, RegistryKeyPermissionCheck.ReadSubTree);
                var result = rk?.GetValue(value) as int?;
                rk?.Close();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
