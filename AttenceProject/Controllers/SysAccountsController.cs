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
    public class SysAccountsController : Controller
    {
        private SysAccountContext db = new SysAccountContext();

        // GET: SysAccounts
        public ActionResult Index()
        {
            return View(db.SysAccounts.ToList());
        }

        // GET: SysAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAccount sysAccount = db.SysAccounts.Find(id);
            if (sysAccount == null)
            {
                return HttpNotFound();
            }
            return View(sysAccount);
        }

        // GET: SysAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysAccounts/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,TimeChange,TimeChangeWay,UsableTime,Operator,OpTime")] SysAccount sysAccount)
        {
            if (ModelState.IsValid)
            {
                db.SysAccounts.Add(sysAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysAccount);
        }

        // GET: SysAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAccount sysAccount = db.SysAccounts.Find(id);
            if (sysAccount == null)
            {
                return HttpNotFound();
            }
            return View(sysAccount);
        }

        // POST: SysAccounts/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,TimeChange,TimeChangeWay,UsableTime,Operator,OpTime")] SysAccount sysAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysAccount);
        }

        // GET: SysAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAccount sysAccount = db.SysAccounts.Find(id);
            if (sysAccount == null)
            {
                return HttpNotFound();
            }
            return View(sysAccount);
        }

        // POST: SysAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysAccount sysAccount = db.SysAccounts.Find(id);
            db.SysAccounts.Remove(sysAccount);
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
