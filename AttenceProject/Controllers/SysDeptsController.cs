using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttenceProject.Models;
using AttenceProject.App_Start;
using System.Text;

namespace AttenceProject.Controllers
{
    public class SysDeptsController : Controller
    {
        private SysDeptContext db = new SysDeptContext();

        // GET: SysDepts
        public ActionResult Index()
        {
            return View();
        }

        // GET: SysDepts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDept sysDept = db.SysDepts.Find(id);
            if (sysDept == null)
            {
                return HttpNotFound();
            }
            return View(sysDept);
        }

        // GET: SysDepts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysDepts/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeptName,ParentNode,DeptOrder,DeptRole,CopyFor,Operator,OpTime")] SysDept sysDept)
        {
            if (ModelState.IsValid)
            {
                db.SysDepts.Add(sysDept);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysDept);
        }

        // GET: SysDepts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDept sysDept = db.SysDepts.Find(id);
            if (sysDept == null)
            {
                return HttpNotFound();
            }
            return View(sysDept);
        }

        // POST: SysDepts/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DeptName,ParentNode,DeptOrder,DeptRole,CopyFor,Operator,OpTime")] SysDept sysDept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysDept).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysDept);
        }

        // GET: SysDepts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDept sysDept = db.SysDepts.Find(id);
            if (sysDept == null)
            {
                return HttpNotFound();
            }
            return View(sysDept);
        }

        // POST: SysDepts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysDept sysDept = db.SysDepts.Find(id);
            db.SysDepts.Remove(sysDept);
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

        public ActionResult GetDeptJson()
        {
            string result = DataTable2Json.LI2J(db.SysDepts.ToList());
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            StringBuilder sbnew = new StringBuilder();

            sbnew = sb.Replace("{\"ID\":", "{\"id\":").Replace(",\"DeptName\":", ",\"name\":").Replace(",\"ParentNode\":", ",\"pId\":");
            return Content(sbnew.ToString());
        }
    }
}
