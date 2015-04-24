using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Windows.Threading;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Syntax;
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

    internal class RemoteBridge : MarshalByRefObject, ITSQLFlexWindow
    {
        
        private readonly Dispatcher m_Dispatcher;
        private readonly ITSQLFlexWindow m_TSQLFlexWindow;


        public RemoteBridge(ITSQLFlexWindow tSqlFlexWindow, Dispatcher dispatcher)
        {
            m_TSQLFlexWindow = tSqlFlexWindow;
            m_TSQLFlexWindow.ConnectionChanged += ConnectionChanged;
            m_Dispatcher = dispatcher;

            //left off here.
        }

        public event EventHandler ConnectionChanged;
        public SqlConnectionStringBuilder CurrentConnection()
        {
            return m_TSQLFlexWindow.CurrentConnection();
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

    public interface ITSQLFlexWindow : ISponsor
    {
        event EventHandler ConnectionChanged;
        SqlConnectionStringBuilder CurrentConnection();
    }


    public static class ObjectFactory
    {
        private static readonly IKernel s_Kernel = new StandardKernel(new NinjectSettings
        {
            InjectNonPublic = true
        }, new HostModule());

        public static T Get<T>()
        {
            return s_Kernel.Get<T>();
        }

        public static IBindingToSyntax<T> Bind<T>()
        {
            if (s_Kernel.GetBindings(typeof(T)).Any())
            {
                throw new InvalidOperationException(String.Format("Binding already exists for type [{0}]", typeof(T)));
            }

            return s_Kernel.Bind<T>();
        }
    }

    internal class HostModule : NinjectModule
    {
        public override void Load()
        {
            Bind<RemoteBridge>().ToSelf().InSingletonScope();
        }
    }

}
