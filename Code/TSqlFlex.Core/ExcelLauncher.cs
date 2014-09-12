using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;

namespace TSqlFlex.Core
{
    public class ExcelLauncher
    {
        private bool excelFoundViaRegistry;
        private string excelLaunchPath;
        private string excelError;

        public ExcelLauncher()
        {
            try 
	        {
                this.excelFoundViaRegistry = false;
                this.excelError = "";
                this.excelLaunchPath = "";

                findExcelViaRegistry();
	        }
	        catch (Exception ex)
	        {
                this.excelFoundViaRegistry = false;
                this.excelLaunchPath = "";
                this.excelError = ex.Message;
	        }
            
        }

        private void findExcelViaRegistry()
        {
            var currVerKey = Registry.ClassesRoot.OpenSubKey("Excel.Sheet\\CurVer");
            if (currVerKey == null)
            {
                excelError = "Can't detect Excel version - no Excel.Sheet\\CurVer in HKCR.";
                return;
            }

            string currVerCode = currVerKey.GetValue("").ToString();
            if (string.IsNullOrEmpty(currVerCode))
            {
                excelError = "Can't detect Excel version - no version in Excel.Sheet\\CurVer.";
                return;
            }

            var cmdKey = Registry.ClassesRoot.OpenSubKey(currVerCode + "\\shell\\Edit\\command");
            if (cmdKey == null)
            {
                excelError = "Couldn't find shell command for Excel version " + currVerCode + ".";
                return;
            }

            string excelCommand = cmdKey.GetValue("").ToString();

            int exeIndex = CultureInfo.InvariantCulture.CompareInfo
                .IndexOf(excelCommand, ".exe", CompareOptions.IgnoreCase);

            if (exeIndex <= "excel".Length)
            {
                excelError = "Couldn't process Excel.exe name from registry.";
                return;
            }

            this.excelLaunchPath = balanceDoubleQuotes(excelCommand.Substring(0, exeIndex + ".exe".Length));

            this.excelFoundViaRegistry = (excelLaunchPath.Length > 0
                && excelError.Length == 0);

        }

        private static string balanceDoubleQuotes(string balanceThis)
        {
            int count = 0;
            char[] characters = balanceThis.ToCharArray();
            for (int i = 0; i < characters.Length; i += 1)
            {
                if (characters[i] == '"')
                {
                    count += 1;
                }
            }
            if (IsOdd(count))
            {
                return balanceThis + "\"";
            }
            return balanceThis;
        }

        private static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public void Launch(string FilePathToOpen)
        {
            Process.Start(this.excelLaunchPath, quoteThePath(FilePathToOpen));
        }


        internal static string quoteThePath(string FilePath)
        {
            if (FilePath.Contains("\""))
            {
                return FilePath;
            }
            return "\"" + FilePath + "\"";
        }

        public bool ExcelFound {
            get {
                return excelFoundViaRegistry;
            }
        }

        public string ExcelError
        {
            get
            {
                return excelError;
            }
        }

    }
}
