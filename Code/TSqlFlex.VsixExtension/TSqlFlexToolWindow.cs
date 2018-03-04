using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using System.Windows.Forms;
using TSqlFlex.Core;

namespace TSqlFlex.VsixExtension
{
    [Guid("35098171-aad6-41fb-80ec-1c6b5eef0fc5")]
    public class TSqlFlexToolWindow : ToolWindowPane
    {
        public FlexMainWindow control;

        public TSqlFlexToolWindow() : base(null)
        {
            Caption = "T-SQL Flex";

            var config = new Config();
            var logger = new Logging(config);
            var ctrl = new FlexMainWindow(logger, config);
            
            Content = new ContainerControl(ctrl);
        }
    }
}
