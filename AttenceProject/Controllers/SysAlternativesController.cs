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
using AttenceProject.Services.Face;
using AttenceProject.Services.Impl;

namespace AttenceProject.Controllers
{
    public class SysAlternativesController : Controller
    {
        private SysAlternativeContext db = new SysAlternativeContext();
        public ISysAlternative service { get; set; }
        // GET: SysAlternatives
        public ActionResult Index()
        {
            return View(db.SysAlternatives.ToList());
        }


        // GET: SysAlternatives/Delete
        public ActionResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string id = ids.TrimStart('[').TrimEnd(']');
            SysAlternative sysAlternative = null;
            for (int i = 0; i < id.Split(',').Count(); i++)
            {
                sysAlternative = db.SysAlternatives.Find(int.Parse(id));
                if (sysAlternative == null)
                {
                    continue;
                }
                db.SysAlternatives.Remove(sysAlternative);
                db.SaveChanges();
            }

            return Content("success");
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
        public ContentResult GetJson(string AlternativeGroupID, string AlternativeText)
        {
            var res = new ContentResult();

            //var list = service.GetSysAlternativesCondition(AlternativeGroupID, AlternativeText);
            var list = db.SysAlternatives.ToList();
            if (AlternativeGroupID != "0" && !string.IsNullOrEmpty(AlternativeGroupID))
            {
                list = list.Where(m => m.AlternativeGroupID == int.Parse(AlternativeGroupID)).ToList();
            }
            if (!string.IsNullOrEmpty(AlternativeText))
            {
                list = list.Where(m => m.AlternativeText.Contains(AlternativeText)).ToList();
            }
            string result = DataTable2Json.LI2J(list);
            result = "{\"total\":" + db.SysAlternatives.ToList().Count + ",\"rows\":" + result + "}";
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
            res.Content = DataTable2Json.EN2J(sysAlternative);
            //res.Content = sysAlternative.AlternativeText + "_" + sysAlternative.AlternativeGroupText + "_" + sysAlternative.Remarks;
            res.ContentType = "application/json";
            res.ContentEncoding = Encoding.UTF8;

            return res;
        }
        /// <summary>
        /// 保存备选项数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="AlternativeText"></param>
        /// <param name="AlternativeGroupText"></param>
        /// <param name="Remarks"></param>
        /// <param name="AlternativeGroupID"></param>
        /// <returns></returns>
        public ActionResult SaveEdit(string ID, string AlternativeText, string Remarks, string AlternativeGroupID)
        {
            IList<SysAlternative> list = db.SysAlternatives.Where(m => m.AlternativeGroupID.ToString() == AlternativeGroupID).ToList();
            if (list.Count >0)
            {
                string AlternativeGroupText = list[0].AlternativeGroupText;
                SysAlternative sys = new SysAlternative();
                sys.ID = int.Parse(ID);
                sys.AlternativeText = AlternativeText;
                sys.AlternativeGroupText = AlternativeGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.AlternativeGroupID = int.Parse(AlternativeGroupID);
                db.Entry(sys).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Content("success");
        }

        // Get: SysAlternatives/GetGroup
        /// <summary>
        /// 获得备选项组别
        /// </summary>
        /// <returns></returns>
        public ContentResult GetGroup(int id)
        {
            string result = DataTable2Json.LI2J(db.SysAlternatives.Select(s => new { s.AlternativeGroupID, s.AlternativeGroupText }).Distinct().ToList());
            if (id == 0)
            {
                result = "[{\"AlternativeGroupID\":0,\"AlternativeGroupText\":\"全部分组\"}," + result.TrimStart('[');
            }
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 获得备选项目具体数据
        /// </summary>
        /// <param name="id">备选项组别</param>
        /// <returns></returns>
        public ContentResult GetAlternativeByGroup(int id)
        {
            string result = DataTable2Json.LI2J(db.SysAlternatives.Where(m => m.AlternativeGroupID == id).Select(s => new { s.ID, s.AlternativeText }).ToList());
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="AlternativeText">备选项名称</param>
        /// <param name="Remarks">备选项</param>
        /// <param name="AlternativeGroupID"></param>
        /// <returns></returns>
        public ActionResult SaveAdd(string AlternativeText, string Remarks, string AlternativeGroupID)
        {
            IList<SysAlternative> list = db.SysAlternatives.Where(m => m.AlternativeGroupID.ToString() == AlternativeGroupID).ToList();
            if (list.Count >0)
            {
                string AlternativeGroupText = list[0].AlternativeGroupText;

                SysAlternative sys = new SysAlternative();
                sys.AlternativeText = AlternativeText;
                sys.AlternativeGroupText = AlternativeGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.AlternativeGroupID = int.Parse(AlternativeGroupID);
                db.SysAlternatives.Add(sys);
                db.SaveChanges();
            }
            return Content("success");

        }
    }
}
