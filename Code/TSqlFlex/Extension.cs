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
        public Config config;
        public Logging logger;

        public Extension()
        {
            config = new Config();
            logger = new Logging(config);
        }

        public void OnLoad(ISsmsExtendedFunctionalityProvider provider)
        {
            logger.Log("Extension.OnLoad");
            ssmsProvider = provider as ISsmsFunctionalityProvider6;
            if (ssmsProvider == null)
            {
                string error = "Could not initialize SIP provider for TSqlFlex extension.";
                logger.Log(error);
                throw new ArgumentException(error);
            }
            runCommand = new RunCommand(ssmsProvider, logger, config);
            runCommand.SetSelectedDBNode(currentNode);

            ssmsProvider.AddToolbarItem(runCommand);
            logger.LogVerbose("Extension.OnLoad complete.");
        }
        
        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
            logger.LogVerbose("Extension.OnNodeChanged");
            currentNode = node;
            if (runCommand != null)
            {
                runCommand.SetSelectedDBNode(currentNode);
            }
            logger.LogVerbose("Extension.OnNodeChanged complete.");
        }

        public void OnShutdown()
        {
            logger.Log("Extension.OnShutdown");
            logger = null;
        }

        public string Author { get { return "Steve Ognibene"; } }
        public string Description { get { return "Scripts data to INSERT statements or Excel-compatible spreadsheets."; } }
        public string Name { get { return "T-SQL Flex"; } }
        public string Url { get { return @"https://github.com/nycdotnet/TSqlFlex/"; } }
        public string Version { get { return Info.VersionNumbersOnly(); } }
    }
}
