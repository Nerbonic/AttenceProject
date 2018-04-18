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
    public class SysApplySetsController : Controller
    {
        private SysApplySetContext db = new SysApplySetContext();

        // GET: SysApplySets
        public ActionResult Index()
        {
            return View(db.SysApplySets.ToList());
        }

        // GET: SysApplySets/Delete
        public ActionResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string id = ids.TrimStart('[').TrimEnd(']');
            SysApplySet SysApplySets = null;
            for (int i = 0; i < id.Split(',').Count(); i++)
            {
                SysApplySets = db.SysApplySets.Find(int.Parse(id));
                if (SysApplySets == null)
                {
                    continue;
                }
                db.SysApplySets.Remove(SysApplySets);
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

        // GET:SysApplySets/GetJson
        public ContentResult GetJson(string ApplyGroupID, string ApplyText)
        {
            var res = new ContentResult();

            var list = db.SysApplySets.ToList();
            if (ApplyGroupID != "0" && !string.IsNullOrEmpty(ApplyGroupID))
            {
                list = list.Where(m => m.ApplyGroupID == int.Parse(ApplyGroupID)).ToList();
            }
            if (!string.IsNullOrEmpty(ApplyText))
            {
                list = list.Where(m => m.ApplyText.Contains(ApplyText)).ToList();
            }
            string result = JsonTool.LI2J(list);
            result = "{\"total\":" + db.SysApplySets.ToList().Count + ",\"rows\":" + result + "}";
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }


        // Get: SysApplySets/GetInfo
        public ActionResult GetInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysApplySet SysApplySets = db.SysApplySets.Find(id);
            if (SysApplySets == null)
            {
                return HttpNotFound();
            }
            var res = new ContentResult();
            res.Content = JsonTool.EN2J(SysApplySets);
            res.ContentType = "application/json";
            res.ContentEncoding = Encoding.UTF8;

            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ApplyText"></param>
        /// <param name="Remarks"></param>
        /// <param name="ApplyGroupID"></param>
        /// <returns></returns>
        public ActionResult SaveEdit(string ID, string ApplyText, string Remarks, string ApplyGroupID)
        {
            IList<SysApplySet> list = db.SysApplySets.Where(m => m.ApplyGroupID.ToString() == ApplyGroupID).ToList();
            if (list.Count > 0)
            {
                string ApplyGroupText = list[0].ApplyGroupText;
                SysApplySet sys = db.SysApplySets.Where(m => m.ID.ToString() == ID).ToList()[0];
                sys.ApplyText = ApplyText;
                sys.ApplyGroupText = ApplyGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.ApplyGroupID = int.Parse(ApplyGroupID);
                db.Entry<SysApplySet>(sys).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Content("success");
        }

        // Get: SysApplySets/GetGroup
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult GetGroup(int id)
        {
            string result = JsonTool.LI2J(db.SysApplySets.Select(s => new { s.ApplyGroupID, s.ApplyGroupText }).Distinct().ToList());
            if (id == 0)
            {
                result = "[{\"ApplyGroupID\":0,\"ApplyGroupText\":\"全部分组\"}," + result.TrimStart('[');
            }
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 获得申请备选项目具体数据
        /// </summary>
        /// <param name="id">备选项组别</param>
        /// <returns></returns>
        public ContentResult GetApplySetByGroup(int id)
        {
            string result = JsonTool.LI2J(db.SysApplySets.Where(m => m.ApplyGroupID == id).Select(s => new { s.ID, s.ApplyText }).ToList());
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
        /// <param name="ApplyText"></param>
        /// <param name="Remarks"></param>
        /// <param name="ApplyGroupID"></param>
        /// <returns></returns>
        public ActionResult SaveAdd(string ApplyText, string Remarks, string ApplyGroupID)
        {
            IList<SysApplySet> list = db.SysApplySets.Where(m => m.ApplyGroupID.ToString() == ApplyGroupID).ToList();
            if (list.Count > 0)
            {
                string ApplyGroupText = list[0].ApplyGroupText;

                SysApplySet sys = new SysApplySet();
                sys.ApplyText = ApplyText;
                sys.ApplyGroupText = ApplyGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.ApplyGroupID = int.Parse(ApplyGroupID);
                db.SysApplySets.Add(sys);
                db.SaveChanges();
            }
            return Content("success");

        }
    }
}
