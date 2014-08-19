using System;
using System.Data.SqlClient;
using RedGate.SIPFrameworkShared;

namespace TSqlFlex
{
    class RunCommand : ISharedCommand
    {
        private readonly ISsmsFunctionalityProvider6 ssmsProvider;
        private readonly ICommandImage commandImage = new CommandImageNone();
        private ObjectExplorerNodeDescriptorBase currentNode = null;

        private ISsmsTabPage formWindow;
        private Guid formGuid = new Guid("579fa20c-38cb-4da6-9f57-6651d10e31d0");

        private FlexMainWindow TheWindow()
        {
            try //This can be a race condition so just give up if there's an exception.
            {
                if (formWindow == null)
                {
                    return null;
                }
                return (FlexMainWindow)formWindow.GetUserControl();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetSelectedDBNode(ObjectExplorerNodeDescriptorBase theSelectedNode)
        {
            currentNode = theSelectedNode;

            var objectExplorerNode = currentNode as IOeNode;
            IConnectionInfo ci = null;
            if (objectExplorerNode != null
                    && objectExplorerNode.HasConnection
                    && objectExplorerNode.TryGetConnection(out ci))
            {
                var w = TheWindow();
                if (w != null)
                {
                    w.SetConnection(new SqlConnectionStringBuilder(ci.ConnectionString));
                }
            }
        }

        public RunCommand(ISsmsFunctionalityProvider6 provider)
        {
            ssmsProvider = provider;
            if (ssmsProvider == null)
            {
                throw new ArgumentException("Could not initialize provider for RunCommand.");
            }
        }

        public void Execute()
        {
            if (formWindow == null)
            {
                formWindow = ssmsProvider.CreateTabPage(typeof(FlexMainWindow), Caption, formGuid.ToString());
                SetSelectedDBNode(currentNode);                
            }

            formWindow.Activate();
        }

        public string Name { get { return "Open_TSQL_Flex"; } }
        public string Caption { get { return "T-SQL Flex"; } }
        public string Tooltip { get { return "Runs a command for scripting"; } }
        public ICommandImage Icon { get { return commandImage; } }
        public string[] DefaultBindings { get { return new string[] {}; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }
    }
}
