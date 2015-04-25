using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;

namespace TSqlFlex
{
    public interface IConnectionProxy
    {
        void SetConnection(SqlConnectionStringBuilder sqlConnectionStringBuilder);
        event EventHandler OnConnectionChanged;
    }

    public class ConnectionProxy : MarshalByRefObject, IConnectionProxy, ISponsor
    {

        event EventHandler OnConnectionChanged;
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

        public void SetConnection(SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            EventHandler handler = OnConnectionChanged;
            if (handler != null)
            {
                ConnectionChangedEventArgs connArgs = new ConnectionChangedEventArgs(sqlConnectionStringBuilder);
                handler(this, connArgs);
            }
        }
        
        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromMinutes(1);
        }

        public override object InitializeLifetimeService()
        {
            ILease ret = (ILease)base.InitializeLifetimeService();
            ret.SponsorshipTimeout = TimeSpan.FromMinutes(2);
            ret.Register(this);
            return ret;
        }

    }


    public class ConnectionChangedEventArgs : EventArgs
    {
        public SqlConnectionStringBuilder sqlConnectionStringBuilder;

        public ConnectionChangedEventArgs(SqlConnectionStringBuilder builder)
        {
            this.sqlConnectionStringBuilder = builder;
        }
    }

}
