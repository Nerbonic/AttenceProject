using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttenceProject.Models;

namespace AttenceProject.Controllers
{
    public class SysMenusController : BaseController
    {
        private SysMenuContext db = new SysMenuContext();
        private SysUsersRoleDbContext db_user = new SysUsersRoleDbContext();

        // GET: SysMenus
        public ActionResult Index()
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            string userid = cook.Values["UserID"];
            int userquanxian = db_user.sur.Where(a => a.ID.ToString() == userid).ToList()[0].UserRole;

            return View(db.SysMenus.Where(a => a.Operator == userquanxian.ToString()).ToList());
        }

        // GET: SysMenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysMenu sysMenu = db.SysMenus.Find(id);
            if (sysMenu == null)
            {
                return HttpNotFound();
            }
            return View(sysMenu);
        }

        // GET: SysMenus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysMenus/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MenuText,MenuParentID,Path,Operator,OpTime")] SysMenu sysMenu)
        {
            if (ModelState.IsValid)
            {
                db.SysMenus.Add(sysMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysMenu);
        }

        // GET: SysMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysMenu sysMenu = db.SysMenus.Find(id);
            if (sysMenu == null)
            {
                return HttpNotFound();
            }
            return View(sysMenu);
        }

        // POST: SysMenus/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MenuText,MenuParentID,Path,Operator,OpTime")] SysMenu sysMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysMenu);
        }

        // GET: SysMenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysMenu sysMenu = db.SysMenus.Find(id);
            if (sysMenu == null)
            {
                return HttpNotFound();
            }
            return View(sysMenu);
        }

        // POST: SysMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysMenu sysMenu = db.SysMenus.Find(id);
            db.SysMenus.Remove(sysMenu);
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
