using System;
using RedGate.SIPFrameworkShared;

namespace TSqlFlex
{
    public class Extension : ISsmsAddin
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
        public string Description { get { return "An add-on for Microsoft SQL Server Management Studio that uses the Red Gate SIP Framework"; } }
        public string Name { get { return "T-SQL Flex"; } }
        public string Url { get { return @"https://twitter.com/nycdotnet"; } }
        public string Version { get { return "0.0.1"; } }
    }
}
