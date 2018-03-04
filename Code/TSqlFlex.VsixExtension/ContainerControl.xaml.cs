using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TSqlFlex.VsixExtension
{
    public partial class ContainerControl : System.Windows.Controls.UserControl
    {
        private Control winformsControl;

        public ContainerControl(Control winformsControl)
        {
            InitializeComponent();
            this.winformsControl = winformsControl;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.EnableVisualStyles();
            var host = new WindowsFormsHost();
            host.Child = winformsControl;
            grid.Children.Add(host);
        }
    }
}
