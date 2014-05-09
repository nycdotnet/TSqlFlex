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
        public ObjectExplorerNodeDescriptorBase currentNode = null;

        public RunCommand(ISsmsFunctionalityProvider4 provider)
        {
            ssmsProvider = provider;
            if (ssmsProvider == null)
            {
                throw new ArgumentException("Could not initialize provider for RunCommand.");
            }
        }

        public string Name { get { return "Run_Command_For_Scripting"; } }
        public void Execute()
        {

            var objectExplorerNode = currentNode as IOeNode;
            IConnectionInfo ci = null;
            if (objectExplorerNode == null
                    || !objectExplorerNode.HasConnection
                    || !objectExplorerNode.TryGetConnection(out ci)) //there should be a TryGetConnection2 or just get ride of IConnectionInfo "1"...
            {
                return;
            }

            StringBuilder buffer = new StringBuilder();
            using (SqlConnection conn = new SqlConnection(ci.ConnectionString))
            {
                try
                {
                    if (conn.Database == "")
                    {
                        //no database selected - can't do anything.
                        return;
                    }
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Name FROM MyStuff;", conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            buffer.Append(reader[0] + ",");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Write("Error selecting from the DB:" + ex.Message + "\n" + ex.StackTrace);
                }
            }

            var qwm = ssmsProvider.GetQueryWindowManager();
            ssmsProvider.QueryWindow.OpenNew("Data from MyStuff: " + buffer.ToString());

            
        }

        public string Caption { get { return "TSQLFlex Run Command"; } }
        public string Tooltip { get { return "Runs a command for scripting"; } }
        public ICommandImage Icon { get { return commandImage; } }
        public string[] DefaultBindings { get { return new string[] {}; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }
   
    }
}
