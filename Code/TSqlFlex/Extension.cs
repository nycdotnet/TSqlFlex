using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TSqlFlex
{
    public class Extension : ISsmsAddin4
    {
        private ObjectExplorerNodeDescriptorBase currentNode;
        private ISsmsFunctionalityProvider6 ssmsProvider;
        private RunCommand runCommand;

        public void OnLoad(ISsmsExtendedFunctionalityProvider provider)
        {
            Debug.WriteLine("Extension.OnLoad");
            ssmsProvider = provider as ISsmsFunctionalityProvider6;
            if (ssmsProvider == null)
            {
                string error = "Could not initialize SIP provider for TSqlFlex extension.";
                Debug.WriteLine(error);
                throw new ArgumentException(error);
            }
            runCommand = new RunCommand(ssmsProvider);
            runCommand.SetSelectedDBNode(currentNode);

            ssmsProvider.AddToolbarItem(runCommand);
            Debug.WriteLine("Extension.OnLoad complete.");
        }

        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
            Debug.WriteLine("Extension.OnNodeChanged");
            currentNode = node;
            if (runCommand != null)
            {
                runCommand.SetSelectedDBNode(currentNode);
            }
            Debug.WriteLine("Extension.OnNodeChanged complete.");
        }

        public void OnShutdown()
        {
            Debug.WriteLine("Extension.OnShutdown");
        }

        public string Author { get { return "Steve Ognibene"; } }
        public string Description { get { return "Scripts data to INSERT statements or Excel-compatible spreadsheets."; } }
        public string Name { get { return "T-SQL Flex"; } }
        public string Url { get { return @"https://github.com/nycdotnet/TSqlFlex/"; } }
        public string Version { get { return Info.VersionNumbersOnly(); } }
        public const string UIDllName = "TSqlFlex.Core.dll";
        public const bool Is64Bit = false;
    }

    
    public static class Info
    {
        public static string Version() {

            return "v" + VersionNumbersOnly() + "-alpha";
        }

        public static string VersionNumbersOnly()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
