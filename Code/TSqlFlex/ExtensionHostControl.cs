using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using RedGate.AppHost.Server;

namespace TSqlFlex
{
    [ComVisible(true)]
    public partial class ExtensionHostControl : UserControl
    {
        public ExtensionHostControl()
        {
            var appHostChildHandle = new ChildProcessFactory().Create(Extension.UIDllName, Debugger.IsAttached, Extension.Is64Bit);

            AppHostServices appHostServices = new AppHostServices();

            var element = appHostChildHandle.CreateElement(appHostServices);

            Controls.Add(new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = element
            });
        }
    }
}
