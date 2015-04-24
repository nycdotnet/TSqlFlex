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
        //event EventHandler ConnectionChanged;
        void SetConnection(SqlConnectionStringBuilder sqlConnectionStringBuilder);
        void OnConnectionChanged(ConnectionChangedEventArgs e);
    }

    public class ConnectionProxy : MarshalByRefObject, IConnectionProxy, ISponsor
    {
        public void SetConnection(SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            OnConnectionChanged(new ConnectionChangedEventArgs(sqlConnectionStringBuilder));
        }

        public delegate void ConnectionChangedHandler(object sender, ConnectionChangedEventArgs e);

        public event ConnectionChangedHandler ConnectionChanged;

        public void OnConnectionChanged(ConnectionChangedEventArgs e)
        {
            if (ConnectionChanged != null)
            {
                ConnectionChanged(this, e);
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
        SqlConnectionStringBuilder sqlConnectionStringBuilder;

        public ConnectionChangedEventArgs(SqlConnectionStringBuilder builder)
        {
            this.sqlConnectionStringBuilder = builder;
        }
    }

}
