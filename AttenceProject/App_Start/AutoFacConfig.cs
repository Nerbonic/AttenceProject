using AttenceProject.Services.Face;
using AttenceProject.Services.Impl;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace AttenceProject.App_Start
{
    public static class AutoFacConfig
    {
        public static void Register()
        {
            // then     
            //Create your builder.
            var builder = new ContainerBuilder();

            // Usually you're only interested in exposing the type
            // via its interface:
            builder.RegisterType<LoginImpl>().As<ILogin>();

            // However, if you want BOTH services (not as common)
            // you can say so:
            builder.RegisterType<LoginImpl>().AsSelf().As<ILogin>();
            //LoginImpl result = (LoginImpl)container.Resolve<ILogin>();
            
            //var a = container.ResolveOptional(typeof(IAddService));
            //var obj1 = container.Resolve<IAddService>();
        }
    }
}