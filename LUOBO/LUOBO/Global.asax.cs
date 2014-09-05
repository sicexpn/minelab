using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LUOBO
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

            routes.MapRoute(
                "Platform", // 路由名称
                "luobo/ds/{deviceSerial}/{version}", // 带有参数的 URL
                new { controller = "Platform", action = "DeviceSetting" } // 参数默认值
            );

            routes.MapRoute(
                "Download", // 路由名称
                "luobo/df/{deviceSerial}/{version}/{filename}", // 带有参数的 URL
                new { controller = "Platform", action = "DownloadFile" } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }
    }
}