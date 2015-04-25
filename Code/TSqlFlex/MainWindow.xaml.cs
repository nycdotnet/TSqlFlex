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

        public MainWindow(ConnectionProxy connection)
        {
            InitializeComponent();

            try
            {
                var safeAppHostChildHandle = new ChildProcessFactory().Create(Extension.UIDllName, Debugger.IsAttached, Extension.Is64Bit);

                AppHostServices appHostServices = new AppHostServices();
                appHostServices.RegisterService<ConnectionProxy, IConnectionProxy>(connection);

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

    }


}
