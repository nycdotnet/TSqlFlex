using RedGate.AppHost.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TSqlFlex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        private readonly RemoteBridge m_RemoteBridge;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                m_RemoteBridge = ObjectFactory.Get<RemoteBridge>();

                var safeAppHostChildHandle = new ChildProcessFactory().Create(Extension.UIDllName, Debugger.IsAttached, Extension.Is64Bit);

                AppHostServices appHostServices = new AppHostServices();
                appHostServices.RegisterService<RemoteBridge, ITSQLFlexWindow>(m_RemoteBridge);

                Content = safeAppHostChildHandle.CreateElement(appHostServices);
            }
            catch (Exception e)
            {
                Content = new TextBlock
                {
                    Text = e.ToString()
                };
            }

        }

        public event EventHandler ConnectionChanged;

        internal void SetConnection(SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            OnConnectionChanged(new ConnectionChangedEventArgs(sqlConnectionStringBuilder));
        }

        protected virtual void OnConnectionChanged(ConnectionChangedEventArgs e)
        {
            if (ConnectionChanged != null)
            {
                ConnectionChanged(this, e);
            }
        }


    }

    public class ConnectionChangedEventArgs : EventArgs
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder;

        public ConnectionChangedEventArgs(SqlConnectionStringBuilder builder)
        {
            this.sqlConnectionStringBuilder = builder;
        }
    }

}
