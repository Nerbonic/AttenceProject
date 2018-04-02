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
    public class SysOverTimesController : Controller
    {
        private SysOverTimeContext db = new SysOverTimeContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();

        // GET: SysOverTimes
        public ActionResult Index()
        {
            return View(db.SysOverTimes.ToList());
        }

        // GET: SysOverTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysOverTime sysOverTime = db.SysOverTimes.Find(id);
            if (sysOverTime == null)
            {
                return HttpNotFound();
            }
            return View(sysOverTime);
        }

        // GET: SysOverTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysOverTimes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProposerID,StartTime,EndTime,Time,OverTimeType,Account_Method,OverTimeReason,CopyFor,ApplyStatus,Applyrate,OpTime")] SysOverTime sysOverTime)
        {
            if (ModelState.IsValid)
            {
                db.SysOverTimes.Add(sysOverTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysOverTime);
        }

        // GET: SysOverTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysOverTime sysOverTime = db.SysOverTimes.Find(id);
            if (sysOverTime == null)
            {
                return HttpNotFound();
            }
            return View(sysOverTime);
        }

        // POST: SysOverTimes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProposerID,StartTime,EndTime,Time,OverTimeType,Account_Method,OverTimeReason,CopyFor,ApplyStatus,Applyrate,OpTime")] SysOverTime sysOverTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysOverTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysOverTime);
        }

        // GET: SysOverTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysOverTime sysOverTime = db.SysOverTimes.Find(id);
            if (sysOverTime == null)
            {
                return HttpNotFound();
            }
            return View(sysOverTime);
        }

        // POST: SysOverTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysOverTime sysOverTime = db.SysOverTimes.Find(id);
            db.SysOverTimes.Remove(sysOverTime);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOverTimeType()
        {
            string result = DataTable2Json.LI2J(db_alter.SysAlternatives.Where(m => m.AlternativeGroupText == "加班类型").ToList());
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountMethod()
        {
            string result = DataTable2Json.LI2J(db_alter.SysAlternatives.Where(m => m.AlternativeGroupText == "加班结算类型").ToList());
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTimePoint()
        {
            string result = DataTable2Json.LI2J(db_apply.SysApplySets.Where(m => m.ApplyGroupText == "申请时间点设置").ToList());
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }
    }
}
