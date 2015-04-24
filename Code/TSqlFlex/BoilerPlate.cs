using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Windows.Threading;
using RedGate.AppHost.Interfaces;
using RedGate.SIPFrameworkShared;

namespace TSqlFlex
{
    public class AppHostServices : MarshalByRefObject, IAppHostServices
    {
        private readonly Dictionary<Type, object> m_Services = new Dictionary<Type, object>();

        public void RegisterService<TConcrete, TService>(TConcrete concrete)
            where TConcrete : MarshalByRefObject, TService
        {
            m_Services[typeof(TService)] = concrete;
        }

        public T GetService<T>() where T : class
        {
            var type = typeof(T);

            return m_Services[type] as T;
        }
    }

    internal class RemoteBridge : MarshalByRefObject, IConnectionProxy
    {
        
        private readonly Dispatcher m_Dispatcher;
        private readonly IConnectionProxy m_ConnectionProxy;


        public RemoteBridge(IConnectionProxy conn, Dispatcher dispatcher)
        {
            m_ConnectionProxy = conn;
            m_Dispatcher = dispatcher;
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

    //public interface ITSQLFlexWindow : ISponsor
    //{
    //    event EventHandler ConnectionChanged;
    //    SqlConnectionStringBuilder CurrentConnection();
    //}


}
