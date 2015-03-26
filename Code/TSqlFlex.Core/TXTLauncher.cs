using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSqlFlex.Core
{
    public class TxtLauncher : ExternalProgramLauncher
    {

        public TxtLauncher() {
        }

        protected override void findProgram()
        {
            var associationKey = Registry.ClassesRoot.OpenSubKey(".txt");
            if (associationKey == null)
            {
                programError = "Can't detect .txt extension registration in HKCR.";
                return;
            }

            string association = associationKey.GetValue("").ToString();
            if (string.IsNullOrEmpty(association))
            {
                programError = "Can't detect .txt association in HKCR.";
                return;
            }

            var cmdKey = Registry.ClassesRoot.OpenSubKey(association + "\\shell\\open\\command");
            if (cmdKey == null)
            {
                programError = "Couldn't find shell command for " + association + ".";
                return;
            }

            string txtCommand = cmdKey.GetValue("").ToString();

            int exeIndex = CultureInfo.InvariantCulture.CompareInfo
                .IndexOf(txtCommand, ".exe", CompareOptions.IgnoreCase);

            this.programLaunchPath = balanceDoubleQuotes(txtCommand.Substring(0, exeIndex + ".exe".Length));

            this.programFound = (programLaunchPath.Length > 0
                && programError.Length == 0);
        }
    }
}
