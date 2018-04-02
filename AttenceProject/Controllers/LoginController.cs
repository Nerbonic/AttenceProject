﻿using AttenceProject.Models;
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
                HttpCookie cook = new HttpCookie("userinfo");

                cook.Values.Set("UserID", list[0].ID.ToString());
                cook.Values.Set("UserName", list[0].UserName);
                cook.Values.Set("UserCode", list[0].UserCode);

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