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
    public class SysApprovesController : Controller
    {
        private SysApproveContext db = new SysApproveContext();

        // GET: SysApproves
        public ActionResult Index()
        {
            return View(db.SysApproves.ToList());
        }

        // GET: SysApproves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysApprove sysApprove = db.SysApproves.Find(id);
            if (sysApprove == null)
            {
                return HttpNotFound();
            }
            return View(sysApprove);
        }

        // GET: SysApproves/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysApproves/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ApplyID,ApplyStatus,Applyrate,NowChecker,NextChecker,OpTime")] SysApprove sysApprove)
        {
            if (ModelState.IsValid)
            {
                db.SysApproves.Add(sysApprove);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysApprove);
        }

        // GET: SysApproves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysApprove sysApprove = db.SysApproves.Find(id);
            if (sysApprove == null)
            {
                return HttpNotFound();
            }
            return View(sysApprove);
        }

        // POST: SysApproves/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ApplyID,ApplyStatus,Applyrate,NowChecker,NextChecker,OpTime")] SysApprove sysApprove)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysApprove).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysApprove);
        }

        // GET: SysApproves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysApprove sysApprove = db.SysApproves.Find(id);
            if (sysApprove == null)
            {
                return HttpNotFound();
            }
            return View(sysApprove);
        }

        // POST: SysApproves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysApprove sysApprove = db.SysApproves.Find(id);
            db.SysApproves.Remove(sysApprove);
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
