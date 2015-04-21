using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TSqlFlex.Core
{
    /// <summary>
    /// Interaction logic for FlexMainWindowX.xaml
    /// </summary>
    public partial class FlexMainWindowX : UserControl
    {
        public FlexMainWindowX()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello, " + txtTest.Text);
        }
    }
}
