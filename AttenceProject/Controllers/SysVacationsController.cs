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
using System.Collections;
using AttenceProject.Services.Impl;
using AttenceProject.Services.Face;

namespace AttenceProject.Controllers
{
    public class SysVacationsController : Controller
    {
        private SysVacationContext db = new SysVacationContext();
        private SysOverTimeContext db_overtime = new SysOverTimeContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();
        private SysUsersRoleDbContext db_user = new SysUsersRoleDbContext();
        private SysApproveContext db_approve = new SysApproveContext();

        public ISysVacation service_vacation = new SysVacationImpl();


        // GET: SysVacations
        public ActionResult Index()
        {
            return View(db.SysVacationsContext.ToList());
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
        /// 获取请假类型
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVacationType()
        {
            string result = service_vacation.GetAlternativesData("请假类型");
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }
        /// <summary>
        /// 获取可申请的时间点
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTimePoint()
        {
            string result = service_vacation.GetTimePoint("申请时间点设置");
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }



        public ActionResult SaveAdd(string VacationReason, string VacationType, string Emergency, string StartTime, string EndTime, string SendFor, string CopyFor)
        {
            HttpCookie cook = Request.Cookies["userinfo"];//从cook中取用户信息
            service_vacation.SaveAdd(cook, VacationReason, VacationType, Emergency, StartTime, EndTime, SendFor, CopyFor);
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
            var list = service_vacation.GetList(userid);
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

            //取审批时，从所有最新审批中找到符合以下两个条件的审批进度：
            //1、审批尚未完成
            //2、最新进度的NowChecker为登录人的

            string result = service_vacation.GetApprove(cook);
            var res = new ContentResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }
        public ActionResult GetVacationInfo(int id)
        {

            string result = service_vacation.GetVacationInfo(id);
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

        public ActionResult SaveApprove(string applyrate, string applystatus, string applyid)
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            service_vacation.SaveApprove(cook, applyrate, applystatus, applyid);
            return Content("success");

        }

        public ActionResult GetApproveDetail(int id)
        {
            var list = db_approve.SysApproves.ToList();
            var list_user = db_user.sur.ToList();
            var query = from a in list
                        join b in list_user
                        on a.LastChecker equals b.ID
                        where a.ApplyID == id && a.LastChecker != 0 && a.ApplyType == "请假"
                        select new
                        {
                            b.UserName,
                            a.Applyrate,
                            ApplyStatus = a.ApplyStatus == 1 ? "通过" : "驳回"
                        };
            string result = JsonTool.LI2J(query.ToList());
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
        public ActionResult GetApproveCopyJson()
        {
            HttpCookie cook = Request.Cookies["userinfo"];
            string result = service_vacation.GetApproveCopy(cook);
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
