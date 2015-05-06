using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;


namespace TSqlFlex.Core
{
    public class ExcelLauncher : ExternalProgramLauncher
    {
        public ExcelLauncher()
        {
        }

        protected override void findProgram()
        {
            var currVerKey = Registry.ClassesRoot.OpenSubKey("Excel.Sheet\\CurVer");
            if (currVerKey == null)
            {
                programError = "Can't detect Excel version - no Excel.Sheet\\CurVer in HKCR.";
                return;
            }

            string currVerCode = currVerKey.GetValue("").ToString();
            if (string.IsNullOrEmpty(currVerCode))
            {
                programError = "Can't detect Excel version - no version in Excel.Sheet\\CurVer.";
                return;
            }

            var cmdKey = Registry.ClassesRoot.OpenSubKey(currVerCode + "\\shell\\Edit\\command");
            if (cmdKey == null)
            {
                programError = "Couldn't find shell command for Excel version " + currVerCode + ".";
                return;
            }

            string excelCommand = cmdKey.GetValue("").ToString();

            int exeIndex = CultureInfo.InvariantCulture.CompareInfo
                .IndexOf(excelCommand, ".exe", CompareOptions.IgnoreCase);

            if (exeIndex <= "excel".Length)
            {
                programError = "Couldn't process Excel.exe name from registry.";
                return;
            }

            this.programLaunchPath = balanceDoubleQuotes(excelCommand.Substring(0, exeIndex + ".exe".Length));

            this.programFound = (programLaunchPath.Length > 0
                && programError.Length == 0);
        }
    }
}
