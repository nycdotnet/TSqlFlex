using RedGate.AppHost.Server;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();


            /*
             * var appHostChildHandle = new ChildProcessFactory().Create(Extension.UIDllName, Debugger.IsAttached, Extension.Is64Bit);

            AppHostServices appHostServices = new AppHostServices();

            var element = appHostChildHandle.CreateElement(appHostServices);

            Controls.Add(new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = element
            });
             * */

            try
            {
                var safeAppHostChildHandle = new ChildProcessFactory().Create(Extension.UIDllName, Debugger.IsAttached, Extension.Is64Bit);

                AppHostServices appHostServices = new AppHostServices();

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
