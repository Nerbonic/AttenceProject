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
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<LoginImpl>().As<ILogin>().InstancePerLifetimeScope();
            // then     
            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //LoginImpl result = (LoginImpl)container.Resolve<ILogin>();
            
            //var a = container.ResolveOptional(typeof(IAddService));
            //var obj1 = container.Resolve<IAddService>();
        }
    }
}