using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace TSqlFlex.Core
{
    [Serializable()]
    public class ConnectionProxyClient : IConnectionProxy
    {
        public event EventHandler OnConnectionChanged;
        event EventHandler IConnectionProxy.OnConnectionChanged
        {
            add
            {
                if (OnConnectionChanged != null)
                {
                    lock (OnConnectionChanged)
                    {
                        OnConnectionChanged += value;
                    }
                }
                else
                {
                    OnConnectionChanged = new EventHandler(value);
                }
            }
            remove
            {
                if (OnConnectionChanged != null)
                {
                    lock (OnConnectionChanged)
                    {
                        OnConnectionChanged -= value;
                    }
                }
            }
        }

        public void connectionChangedHandler(object sender, EventArgs args)
        {
            SetConnection(((ConnectionChangedEventArgs)args).sqlConnectionStringBuilder);
        }

        public void SetConnection(SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            EventHandler handler = OnConnectionChanged;
            if (handler != null)
            {
                ConnectionChangedEventArgs connArgs = new ConnectionChangedEventArgs(sqlConnectionStringBuilder);
                handler(this, connArgs);
            }
        }

    }
}
