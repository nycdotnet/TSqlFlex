using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TSqlFlex.Core
{
    public abstract class ExternalProgramLauncher
    {
        protected bool programFound;
        protected string programLaunchPath;
        protected string programError;

        protected ExternalProgramLauncher()
        {
            try
            {
                this.programFound = false;
                this.programError = "";
                this.programLaunchPath = "";

                findProgram();
            }
            catch (Exception ex)
            {
                this.programFound = false;
                this.programLaunchPath = "";
                this.programError = ex.Message;
            }
        }

        protected abstract void findProgram();

        public void Launch(string FilePathToOpen)
        {
            Process.Start(this.programLaunchPath, quoteThePath(FilePathToOpen));
        }

        protected static string balanceDoubleQuotes(string balanceThis)
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


        internal static string quoteThePath(string FilePath)
        {
            if (FilePath.Contains("\""))
            {
                return FilePath;
            }
            return "\"" + FilePath + "\"";
        }

        protected static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public bool ProgramFound
        {
            get
            {
                return programFound;
            }
        }

        public string ProgramError
        {
            get
            {
                return programError;
            }
        }

    }
}
