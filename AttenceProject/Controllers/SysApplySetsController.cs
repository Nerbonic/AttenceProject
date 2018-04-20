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
    public class SysApplySetsController : Controller
    {
        private SysApplySetContext db = new SysApplySetContext();
        public ISysApplySet service_apply = new SysApplySetImpl();

        // GET: SysApplySets
        public ActionResult Index()
        {
            return View(db.SysApplySets.ToList());
        }

        // GET: SysApplySets/Delete
        public ActionResult Delete(string ids)
        {
             string rtrStr=service_apply.Delete(ids);

            return Content(rtrStr);
            //return Content("success");
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

            var list = service_apply.GetSysApplySetsCondition(ApplyGroupID, ApplyText);
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
        public ActionResult GetInfo(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysApplySet SysApplySets = service_apply.GetInfo(id);
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
            service_apply.SaveEdit(ID, ApplyText, Remarks, ApplyGroupID);
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
            string result = service_apply.GetGroup(id);
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
            string result = service_apply.GetApplySetByGroup(id);
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
            service_apply.SaveAdd(ApplyText, Remarks, ApplyGroupID);
            return Content("success");

        }
    }
}
