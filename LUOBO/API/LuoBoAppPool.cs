using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using LuoBo.Api;

namespace LuoBo.Api
{
    public class FrienDevAppPool
    {
        public static FrienDevAppPool Instance = new FrienDevAppPool();

        private Dictionary<string, FrienDevApplication> m_AppPool = new Dictionary<string, FrienDevApplication>();
        private IApiHelper m_ApiHelper;

        private FrienDevAppPool()
        {
        }

        public void SetApiHelper(IApiHelper apiHelper)
        {
            m_ApiHelper = apiHelper;
        }

        public IApiHelper GetApiHelper()
        {
            return m_ApiHelper;
        }

        public FrienDevApplication GetApplication(Type applicationType)
        {
            return GetApplication(applicationType.Assembly.FullName, applicationType.FullName);
        }

        public FrienDevApplication GetApplication(string assemblyName, string className)
        {
            string key = assemblyName + "*" + className;
            if (!m_AppPool.ContainsKey(key))
            {
                ObjectHandle o = Activator.CreateInstance(assemblyName, className);
                FrienDevApplication applicationObject = o.Unwrap() as FrienDevApplication;
                m_AppPool.Add(key, applicationObject);
                return applicationObject;
            }
            else
            {
                return m_AppPool[key];
            }
        }
    }
}
