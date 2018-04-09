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
    public class SysOverTimesController : BaseController
    {
        private SysOverTimeContext db = new SysOverTimeContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();
        private SysUsersRole db_user = new SysUsersRole();
        private SysApproveContext db_approve = new SysApproveContext();
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
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOverTimeType()
        {
            string result = JsonTool.LI2J(db_alter.SysAlternatives.Where(m => m.AlternativeGroupText == "加班类型").ToList());
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
            string result = JsonTool.LI2J(db_alter.SysAlternatives.Where(m => m.AlternativeGroupText == "加班结算类型").ToList());
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
            string result = JsonTool.LI2J(db_apply.SysApplySets.Where(m => m.ApplyGroupText == "申请时间点设置").ToList());
            var res = new ContentResult();
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
        public ActionResult SaveAdd(string OverTimeReason, string OverTimeType, string Account_Method, string StartTime, string EndTime)
        {
            HttpCookie cook = Request.Cookies["userinfo"];//从cook中取用户信息
            string userid = cook.Values["UserID"];
            string username = cook.Values["UserName"];
            string usercode = cook.Values["UserCode"];

            #region 新建加班信息
            SysOverTime sys = new SysOverTime();
            sys.OverTimeReason = OverTimeReason;
            sys.ProposerID = int.Parse(userid);
            sys.OverTimeType = int.Parse(OverTimeType);
            sys.Account_Method = int.Parse(Account_Method);
            sys.ApplyStatus = 0;
            sys.StartTime = DateTime.Parse(StartTime);
            sys.EndTime = DateTime.Parse(EndTime);
            sys.CopyFor = "11_";
            TimeSpan ts1 = sys.EndTime - sys.StartTime;//计算加班总经历时间的时间戳
            int hours = ts1.Hours - 9;//计算加班结束当天的多出来的加班时间
            int days = ts1.Days;//若加班大于1天，计算加班天数
            if (days == 0)
            {
                hours = ts1.Hours;
            }
            sys.Time = days + hours / 24;//计算出总时长，形式：4.2代表4天+0.2天，即4天+4.8小时
            sys.OpTime = DateTime.Now;
            db.SysOverTimes.Add(sys);
            db.SaveChanges();

            #endregion
            
            #region 新建审批流信息
            SysApprove sysapprove = new SysApprove();
            sysapprove.ApplyID = sys.ID;
            sysapprove.ApplyStatus = 0;
            sysapprove.OpTime = DateTime.Now;
            sysapprove.NowChecker = int.Parse(sys.CopyFor.Split('_')[0]);
            #endregion

            return RedirectToAction("List");

        }

        public ActionResult List()
        {

            return View();

        }

        public ActionResult GetJson()
        {

            var res = new ContentResult();

            var list = db.SysOverTimes.ToList();
            //if (AlternativeGroupID != "0" && !string.IsNullOrEmpty(AlternativeGroupID))
            //{
            //    list = list.Where(m => m.AlternativeGroupID == int.Parse(AlternativeGroupID)).ToList();
            //}
            //if (!string.IsNullOrEmpty(AlternativeText))
            //{
            //    list = list.Where(m => m.AlternativeText.Contains(AlternativeText)).ToList();
            //}
            string result = JsonTool.LI2J(list);
            result = "{\"total\":" + db.SysOverTimes.ToList().Count + ",\"rows\":" + result + "}";
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
            var res = new ContentResult();
            res = (ContentResult)GetJson();
            return res;
        }
    }
}
