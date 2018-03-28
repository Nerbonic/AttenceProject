using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttenceProject.Controllers
{
    public class LoginController : BaseController
    {
        private SysUsersRoleDbContext db = new SysUsersRoleDbContext();

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

            IList<SysUsersRole> list = db.sur.Where(m => m.LoginName == username).ToList();
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
                return null;
            }
        }

        public ActionResult toM()
        {
            return View();
        }
    }
}