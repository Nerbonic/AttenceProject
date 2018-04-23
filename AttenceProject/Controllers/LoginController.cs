using AttenceProject.Models;
using AttenceProject.Services.Face;
using AttenceProject.Services.Impl;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttenceProject.Controllers
{
    public class LoginController : BaseController
    {

        ILogin service_login = new LoginImpl();

        public LoginController(ILogin _service_login)
        {
            service_login = _service_login;
        }
        public LoginController()
        {

        }
        // GET: Login
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        // POST:Login/DoLogin
        [HttpPost]
        public ActionResult DoLogin(string username, string password)
        {

            ContainerBuilder builder = new ContainerBuilder();
            builder.regi
            builder.RegisterModule(new LoginImpl());
            //builder.RegisterType<Test>();


            //builder.Register(t => new Test()).As<Test>();
            builder.RegisterType<Test2>();
            var container = builder.Build();

            Test2 test2 = container.Resolve<Test2>();
            // Test2 ee = new Test2();
            test2.Show();
            IList<SysUsersRole> list = service_login.GetUserInfoByName(username);
            //IList<SysUsersRole> list = db.sur.Where(m => m.LoginName == username).ToList();
            if (list.Count == 0)
            {
                return Content("<script>alert('不存在此用户!登陆失败');window.location.href='/Login/Index';</script>");
            }
            else if (list.Where(m => m.PassWord == password).ToList().Count == 0)
            {
                return Content("<script>alert('密码错误!登陆失败');window.location.href='/Login/Index';</script>");
            }
            else
            {
                HttpCookie cook = new HttpCookie("userinfo");
                cook.Values.Set("UserID", list[0].ID.ToString());
                cook.Values.Set("UserName", list[0].UserName);
                cook.Values.Set("UserCode", list[0].UserCode);
                cook.Values.Set("UserRole", list[0].UserRole.ToString());
                cook.Expires.AddDays(1);//设置过期时间  
                Response.SetCookie(cook);//若已有此cookie，更新内容  
                Response.Cookies.Add(cook);//添加此cookie  
                return RedirectToAction("toM", "Login");
            }
        }

        public ActionResult toM()
        {
            return View();
        }
    }
}