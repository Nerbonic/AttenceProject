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
    public class SysAlternativesController : Controller
    {
        private SysAlternativeContext db = new SysAlternativeContext();

        // GET: SysAlternatives
        public ActionResult Index()
        {
            return View(db.SysAlternatives.ToList());
        }

        // GET: SysAlternatives/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAlternative sysAlternative = db.SysAlternatives.Find(id);
            if (sysAlternative == null)
            {
                return HttpNotFound();
            }
            return View(sysAlternative);
        }

        // GET: SysAlternatives/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysAlternatives/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AlternativeText,AlternativeGroupID,AlternativeGroupText,Remarks,Operator,OpTime")] SysAlternative sysAlternative)
        {
            if (ModelState.IsValid)
            {
                db.SysAlternatives.Add(sysAlternative);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysAlternative);
        }

        // GET: SysAlternatives/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAlternative sysAlternative = db.SysAlternatives.Find(id);
            if (sysAlternative == null)
            {
                return HttpNotFound();
            }
            var res = new ContentResult();
            res.Content = sysAlternative.AlternativeText+"_"+sysAlternative.Remarks+"_"+sysAlternative.AlternativeGroupText;
            res.ContentType = "application/json";
            res.ContentEncoding = Encoding.UTF8;

            return res;
        }

        // POST: SysAlternatives/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AlternativeText,AlternativeGroupID,AlternativeGroupText,Remarks,Operator,OpTime")] SysAlternative sysAlternative)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysAlternative).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysAlternative);
        }

        // GET: SysAlternatives/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAlternative sysAlternative = db.SysAlternatives.Find(id);
            if (sysAlternative == null)
            {
                return HttpNotFound();
            }
            return View(sysAlternative);
        }

        // POST: SysAlternatives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysAlternative sysAlternative = db.SysAlternatives.Find(id);
            db.SysAlternatives.Remove(sysAlternative);
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

        // GET:SysAlternatives/GetJson
        public ContentResult GetJson()
        {
            var res = new ContentResult();
            var list = db.SysAlternatives.ToList();
            string result = DataTable2Json.LI2J(db.SysAlternatives.ToList());
            result = "{\"total\":"+ db.SysAlternatives.ToList().Count+ ",\"rows\":" + result + "}";
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }


        // Get: SysAlternatives/GetInfo
        public ActionResult GetInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAlternative sysAlternative = db.SysAlternatives.Find(id);
            if (sysAlternative == null)
            {
                return HttpNotFound();
            }
            var res = new ContentResult();
            res.Content = sysAlternative.AlternativeText + "_" + sysAlternative.AlternativeGroupText + "_" + sysAlternative.Remarks;
            res.ContentType = "application/json";
            res.ContentEncoding = Encoding.UTF8;

            return res;
        }
    }
}
