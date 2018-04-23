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
using AttenceProject.Models.ViewModel;
using System.Collections;
using AttenceProject.Services.Face;
using AttenceProject.Services.Impl;

namespace AttenceProject.Controllers
{
    public class SysOverTimesController : BaseController
    {
        private SysOverTimeContext db = new SysOverTimeContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();
        private SysUsersRoleDbContext db_user = new SysUsersRoleDbContext();
        private SysApproveContext db_approve = new SysApproveContext();

        public ISysOverTime service_overtime = new SysOverImpl();
        // GET: SysOverTimes
        public ActionResult Index()
        {
            return View(db.SysOverTimes.ToList());
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
        /// 获取加班类型
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOverTimeType()
        {
            string result = service_overtime.GetAlternativesData("加班类型");
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
            string result = service_overtime.GetAlternativesData("加班结算类型");
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
            string result = service_overtime.GetTimePoint("申请时间点设置");            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 保存加班信息
        /// </summary>
        /// <param name="OverTimeReason">加班理由</param>
        /// <param name="OverTimeType">加班类型</param>
        /// <param name="Account_Method">加班结算类型</param>
        /// <param name="StartTime">加班开始时间</param>
        /// <param name="EndTime">加班结束时间</param>
        /// <returns></returns>
        public ActionResult SaveAdd(string OverTimeReason, string OverTimeType, string Account_Method, string StartTime, string EndTime,string SendFor,string CopyFor)
        {

            HttpCookie cook = Request.Cookies["userinfo"];//从cook中取用户信息
            service_overtime.SaveAdd(cook, OverTimeReason, OverTimeType, Account_Method, StartTime, EndTime, SendFor, CopyFor);
            return RedirectToAction("List");

        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult GetJson()
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            var res = new ContentResult();
            string userid = cook.Values["UserID"];
            var list = service_overtime.GetList(userid);
            //if (AlternativeGroupID != "0" && !string.IsNullOrEmpty(AlternativeGroupID))
            //{
            //    list = list.Where(m => m.AlternativeGroupID == int.Parse(AlternativeGroupID)).ToList();
            //}
            //if (!string.IsNullOrEmpty(AlternativeText))
            //{
            //    list = list.Where(m => m.AlternativeText.Contains(AlternativeText)).ToList();
            //}
            string result = JsonTool.LI2J(list.ToList());
            result = "{\"total\":" + list.Count + ",\"rows\":" + result + "}";
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;

        }

        public ActionResult GetApproveCopyJson()
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            var res = new ContentResult();
            string userid = cook.Values["UserID"];
            var list = service_overtime.GetCopyList(userid);
            //if (AlternativeGroupID != "0" && !string.IsNullOrEmpty(AlternativeGroupID))
            //{
            //    list = list.Where(m => m.AlternativeGroupID == int.Parse(AlternativeGroupID)).ToList();
            //}
            //if (!string.IsNullOrEmpty(AlternativeText))
            //{
            //    list = list.Where(m => m.AlternativeText.Contains(AlternativeText)).ToList();
            //}
            string result = JsonTool.LI2J(list.ToList());
            result = "{\"total\":" + list.Count + ",\"rows\":" + result + "}";
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;

        }

        public ActionResult Approve()
        {
            return View();
        }

        public ActionResult ApproveCopy()
        {
            return View();
        }
        public ActionResult GetApprove()
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            string result= service_overtime.GetApprove(cook);
            var res = new ContentResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 获取加班数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetOverTimeInfo(int id)
        {
            string result = service_overtime.GetOverTimeInfo(id);
            if (string.IsNullOrEmpty(result))
            {
                return HttpNotFound();
            }
            var res = new ContentResult();
            res.Content = result.TrimStart('[').TrimEnd(']');
            //res.Content = sysAlternative.AlternativeText + "_" + sysAlternative.AlternativeGroupText + "_" + sysAlternative.Remarks;
            res.ContentType = "application/json";
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 保存审批时的审批数据
        /// </summary>
        /// <param name="applyrate"></param>
        /// <param name="applystatus"></param>
        /// <param name="applyid"></param>
        /// <returns></returns>
        public ActionResult SaveApprove(string applyrate,string applystatus,string applyid)
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            service_overtime.SaveApprove(cook, applyrate, applystatus, applyid);
            return Content("success");
        }



        /// <summary>
        /// 获取审批的通过详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetApproveDetail(int id)
        {

            string result = service_overtime.GetApproveDetail(id);
            if (string.IsNullOrEmpty(result))
            {
                return HttpNotFound();
            }
            var res = new ContentResult();
            res.Content = result;
            //res.Content = sysAlternative.AlternativeText + "_" + sysAlternative.AlternativeGroupText + "_" + sysAlternative.Remarks;
            res.ContentType = "application/json";
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 获取抄送到的申请数据
        /// </summary>
        /// <param name="row">一页的行数</param>
        /// <param name="page">第几页</param>
        /// <returns></returns>
        public ActionResult GetApproveCopy()
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            string result = service_overtime.GetApproveCopy(cook);
            var res = new ContentResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

    }
}
