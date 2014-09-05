using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace LUOBO.Service
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.AbsolutePath.IndexOf("WinXinService/ValidUr") > -1)
            {
                HttpContext.Current.Request.ContentType = "";
            }
        }

        private void RegisterRoutes()
        {
            // Edit the base address of Service1 by replacing the "Service1" string below
            RouteTable.Routes.Add(new ServiceRoute("LUOBOService", new WebServiceHostFactory(), typeof(LUOBOService)));
            RouteTable.Routes.Add(new ServiceRoute("DeviceService", new WebServiceHostFactory(), typeof(DeviceService)));
            RouteTable.Routes.Add(new ServiceRoute("ConfigService", new WebServiceHostFactory(), typeof(ConfigService)));
            RouteTable.Routes.Add(new ServiceRoute("WinXinService", new WebServiceHostFactory(), typeof(WinXinService)));
        }
    }
}
