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
    public class SysWorkOffsController : Controller
    {
        private SysWorkOffContext db = new SysWorkOffContext();

        // GET: SysWorkOffs
        public ActionResult Index()
        {
            return View(db.SysWorkOffs.ToList());
        }

        // GET: SysWorkOffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysWorkOff sysWorkOff = db.SysWorkOffs.Find(id);
            if (sysWorkOff == null)
            {
                return HttpNotFound();
            }
            return View(sysWorkOff);
        }

        // GET: SysWorkOffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysWorkOffs/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProposerID,WorkOffType,OverTimeStart,OverTimeEnd,StartTime,EndTime,Time,VacationReason,Emergency,copyFor,ApplyStatus,Applyrate,Operator,OpTime")] SysWorkOff sysWorkOff)
        {
            if (ModelState.IsValid)
            {
                db.SysWorkOffs.Add(sysWorkOff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysWorkOff);
        }

        // GET: SysWorkOffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysWorkOff sysWorkOff = db.SysWorkOffs.Find(id);
            if (sysWorkOff == null)
            {
                return HttpNotFound();
            }
            return View(sysWorkOff);
        }

        // POST: SysWorkOffs/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProposerID,WorkOffType,OverTimeStart,OverTimeEnd,StartTime,EndTime,Time,VacationReason,Emergency,copyFor,ApplyStatus,Applyrate,Operator,OpTime")] SysWorkOff sysWorkOff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysWorkOff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysWorkOff);
        }

        // GET: SysWorkOffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysWorkOff sysWorkOff = db.SysWorkOffs.Find(id);
            if (sysWorkOff == null)
            {
                return HttpNotFound();
            }
            return View(sysWorkOff);
        }

        // POST: SysWorkOffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysWorkOff sysWorkOff = db.SysWorkOffs.Find(id);
            db.SysWorkOffs.Remove(sysWorkOff);
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


        public ActionResult StartWorkOff()
        {
            return View();
        }
    }
}
