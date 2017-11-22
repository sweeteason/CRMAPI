using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CRMAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "NoStatusApi",
                routeTemplate: "{controller}/{action}/{id}",
                   defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
