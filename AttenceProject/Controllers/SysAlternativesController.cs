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


        public ISysAlternative service_alter = new SysAlternativeImpl();
        // GET: SysAlternatives
        public ActionResult Index()
        {
            return View(service_alter.GetSysAlternatives());
        }


        // GET: SysAlternatives/Delete
        public ActionResult Delete(string ids)
        {
            string rtrStr=service_alter.Delete(ids);

            return Content(rtrStr);
        }


        // GET:SysAlternatives/GetJson
        public ContentResult GetJson(string AlternativeGroupID, string AlternativeText)
        {
            var res = new ContentResult();

            var list = service_alter.GetSysAlternativesCondition(AlternativeGroupID, AlternativeText);
            
            string result = JsonTool.LI2J(list);
            result = "{\"total\":" + list.Count + ",\"rows\":" + result + "}";
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }


        // Get: SysAlternatives/GetInfo
        public ActionResult GetInfo(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysAlternative sysAlternative = service_alter.GetInfo(id);
            if (sysAlternative == null)
            {
                return HttpNotFound();
            }
            var res = new ContentResult();
            res.Content = JsonTool.EN2J(sysAlternative);
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
            service_alter.SaveEdit(ID, AlternativeText, Remarks, AlternativeGroupID);
            return Content("success");
        }

        // Get: SysAlternatives/GetGroup
        /// <summary>
        /// 获得备选项组别
        /// </summary>
        /// <returns></returns>
        public ContentResult GetGroup(int id)
        {
            string result = service_alter.GetGroup(id);
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
            string result = service_alter.GetAlternativbeByGroup(id);
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
            service_alter.SaveAdd(AlternativeText, Remarks, AlternativeGroupID);
            return Content("success");

        }
    }
}
