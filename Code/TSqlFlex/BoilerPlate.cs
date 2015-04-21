using RedGate.AppHost.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

}
