using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttenceProject;
using AttenceProject.Models;

namespace AttenceProject.Controllers
{
    public class SysUsersRolesController : Controller
    {
        private SysUsersRoleDbContext db = new SysUsersRoleDbContext();

        // GET: SysUsersRoles
        public ActionResult Index()
        {
            return View(db.sur.ToList());
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
