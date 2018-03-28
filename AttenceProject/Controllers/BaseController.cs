using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttenceProject.Controllers
{
    public abstract class BaseController : Controller
    {
        private SysMenuContext db = new SysMenuContext();
        public BaseController()
        {
            ViewData["menus"] = db.SysMenus.Where(m => m.MenuParentID == 0).ToList();
            ViewData["sonmenus"] = db.SysMenus.Where(m => m.MenuParentID != 0).ToList();
        }

    }
}