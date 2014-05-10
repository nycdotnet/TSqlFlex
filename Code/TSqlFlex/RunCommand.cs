using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using RedGate.SIPFrameworkShared;

namespace TSqlFlex
{
    class RunCommand : ISharedCommand
    {
        private readonly ISsmsFunctionalityProvider4 ssmsProvider;
        private readonly ICommandImage commandImage = new CommandImageNone();
        private ObjectExplorerNodeDescriptorBase currentNode = null;
        private frmMain form = new frmMain();

        public void SetSelectedDBNode(ObjectExplorerNodeDescriptorBase theSelectedNode)
        {
            currentNode = theSelectedNode;

            var objectExplorerNode = currentNode as IOeNode;
            IConnectionInfo ci = null;
            if (objectExplorerNode != null
                    && objectExplorerNode.HasConnection
                    && objectExplorerNode.TryGetConnection(out ci)) //there should be a TryGetConnection2 or just get rid of IConnectionInfo "1"...
            {
                form.SetConnection(new SqlConnectionStringBuilder(ci.ConnectionString));
            }
        }

        public RunCommand(ISsmsFunctionalityProvider4 provider)
        {
            ssmsProvider = provider;
            if (ssmsProvider == null)
            {
                throw new ArgumentException("Could not initialize provider for RunCommand.");
            }
        }

        public string Name { get { return "Open_TSQL_Flex"; } }
        public void Execute()
        {
            if (form == null || form.IsDisposed)
            {
                form = new frmMain();
                SetSelectedDBNode(currentNode);
            }
            form.Show();
        }

        public string Caption { get { return "TSQLFlex"; } }
        public string Tooltip { get { return "Runs a command for scripting"; } }
        public ICommandImage Icon { get { return commandImage; } }
        public string[] DefaultBindings { get { return new string[] {}; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }
    }
}
