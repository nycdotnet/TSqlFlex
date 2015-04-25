using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RedGate.AppHost.Interfaces;
using System.Windows.Forms.Integration;
using System.Data.SqlClient;

namespace TSqlFlex.Core
{
    class OutOfProcessEntryPoint : IOutOfProcessEntryPoint
    {
        private FlexMainWindowX m_MainWindowControl;
        private IConnectionProxy m_connectionProxy;

        public FrameworkElement CreateElement(IAppHostServices service)
        {
            m_connectionProxy = service.GetService<IConnectionProxy>();
            m_MainWindowControl = new FlexMainWindowX(m_connectionProxy);

            return m_MainWindowControl;
        }
    }

}
