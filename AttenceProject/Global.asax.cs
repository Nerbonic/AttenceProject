using AttenceProject.App_Core;
using AttenceProject.Services.Face;
using AttenceProject.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;

namespace AttenceProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //启用压缩
            //BundleTable.EnableOptimizations = true;
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            //注入 Ioc

        }
        IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<ISysAlternative, SysAlternativeImpl>();
            return container;
        }

    }
}
