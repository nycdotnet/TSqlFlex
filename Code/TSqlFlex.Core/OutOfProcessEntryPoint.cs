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
        private ITSQLFlexWindow tSqlFlexWindow;

        public FrameworkElement CreateElement(IAppHostServices service)
        {

            m_MainWindowControl = new FlexMainWindowX();
            this.tSqlFlexWindow = service.GetService<ITSQLFlexWindow>();

            //ObjectFactory.Bind<ITSQLFlexWindow>().ToConstant(tSqlFlexWindow).InSingletonScope();

            return m_MainWindowControl;
        }
    }

}
