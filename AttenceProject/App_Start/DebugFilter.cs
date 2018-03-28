using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttenceProject.App_Start
{
    public class DebugFilter:ActionFilterAttribute
    {
        private IDebugMessageService debugMessageService;

        //构造函数注入
        public DebugFilter(IDebugMessageService debugMessageService)
        {
            this.debugMessageService = debugMessageService;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write(debugMessageService.Message);
        }


    }

    public interface IDebugMessageService
    {
        string Message { get; }
    }

    public class DebugMessageService : IDebugMessageService
    {
        public string Message
        {
            get
            {
                return "Debugging...";
            }
        }
    }
}