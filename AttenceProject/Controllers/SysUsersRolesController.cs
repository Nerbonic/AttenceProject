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

        // GET: SysUsersRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysUsersRole sysUsersRole = db.sur.Find(id);
            if (sysUsersRole == null)
            {
                return HttpNotFound();
            }
            return View(sysUsersRole);
        }

        // GET: SysUsersRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysUsersRoles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,UserCode,UserDeptID,LoginName,PassWord,UserState,UserRole,OverTime,Operator,OpTime")] SysUsersRole sysUsersRole)
        {
            if (ModelState.IsValid)
            {
                db.sur.Add(sysUsersRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysUsersRole);
        }

        // GET: SysUsersRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysUsersRole sysUsersRole = db.sur.Find(id);
            if (sysUsersRole == null)
            {
                return HttpNotFound();
            }
            return View(sysUsersRole);
        }

        // POST: SysUsersRoles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,UserCode,UserDeptID,LoginName,PassWord,UserState,UserRole,OverTime,Operator,OpTime")] SysUsersRole sysUsersRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysUsersRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysUsersRole);
        }

        // GET: SysUsersRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysUsersRole sysUsersRole = db.sur.Find(id);
            if (sysUsersRole == null)
            {
                return HttpNotFound();
            }
            return View(sysUsersRole);
        }

        // POST: SysUsersRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysUsersRole sysUsersRole = db.sur.Find(id);
            db.sur.Remove(sysUsersRole);
            db.SaveChanges();
            return RedirectToAction("Index");
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
