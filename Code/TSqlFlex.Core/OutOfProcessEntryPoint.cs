using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RedGate.AppHost.Interfaces;
using System.Windows.Forms.Integration;

namespace TSqlFlex.Core
{
    class OutOfProcessEntryPoint : IOutOfProcessEntryPoint
    {
        private FlexMainWindowX m_MainWindowControl;

        public FrameworkElement CreateElement(IAppHostServices service)
        {
            //m_MainWindowControl = new FlexMainWindow();

            //return new WindowsFormsHost
            //{
            //    Child = m_MainWindowControl
            //};

            m_MainWindowControl = new FlexMainWindowX();

            return m_MainWindowControl;
        }
    }
}
