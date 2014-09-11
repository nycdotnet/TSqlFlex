using System;
using RedGate.SIPFrameworkShared;
using TSqlFlex.Core;

namespace TSqlFlex
{
    public class Extension : ISsmsAddin4
    {
        private ObjectExplorerNodeDescriptorBase currentNode;
        private ISsmsFunctionalityProvider6 ssmsProvider;
        private RunCommand runCommand;

        public void OnLoad(ISsmsExtendedFunctionalityProvider provider)
        {
            ssmsProvider = provider as ISsmsFunctionalityProvider6;
            if (ssmsProvider == null)
            {
                throw new ArgumentException("Could not initialize SIP provider for TSqlFlex extension.");
            }
            runCommand = new RunCommand(ssmsProvider);
            runCommand.SetSelectedDBNode(currentNode);

            ssmsProvider.AddToolbarItem(runCommand);
        }
        
        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
            currentNode = node;
            if (runCommand != null)
            {
                runCommand.SetSelectedDBNode(currentNode);
            }
        }

        public void OnShutdown()
        {
        }

        public string Author { get { return "Steve Ognibene"; } }
        public string Description { get { return "Scripts data to INSERT statements or Excel-compatible spreadsheets."; } }
        public string Name { get { return "T-SQL Flex"; } }
        public string Url { get { return @"https://github.com/nycdotnet/TSqlFlex/"; } }
        public string Version { get { return Info.VersionNumbersOnly(); } }
    }
}
