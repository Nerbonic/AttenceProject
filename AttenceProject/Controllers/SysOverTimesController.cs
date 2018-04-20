﻿using System;
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
            IList<SysApprove> list = db_approve.SysApproves.Where(m => m.ApplyID.ToString() == applyid).ToList();
            SysOverTime sys_overtime = db.SysOverTimes.Where(s => s.ID.ToString() == applyid).ToList()[0];
            //获取请假信息及最近一次的审批信息

            string sendfor = sys_overtime.SendFor;
            string[] sendfors = sendfor.TrimEnd('_').Split('_');
            IList sendlist = sendfors.ToList();

            SysApprove sys = new SysApprove();
            SysApprove sys_first = list[0];
            sys.ApplyID = int.Parse(applyid);
            sys.Applyrate = applyrate;
            sys.ApplyStatus = int.Parse(applystatus);
            sys.OpTime = DateTime.Now;
            sys.ApplyType = "加班";
            
            HttpCookie cook = Request.Cookies["userinfo"];
            int NowCheckerIndex = sendlist.IndexOf(cook.Values["UserID"]);
            if (NowCheckerIndex == sendfors.Length - 1)
            {
                //如果是最后一个人的审批的此条申请
                //保存最后一个人的审批信息
                sys.NextChecker = 0;
                sys.LastChecker = int.Parse(sendfors[NowCheckerIndex]);
                sys.NowChecker = 0;
                db_approve.SysApproves.Add(sys);
                db_approve.SaveChanges();

                //结束一个审批流程：
                //结束审批流程要做的事情：修改申请信息为通过：
                SysOverTime sys_overtime_base = db.SysOverTimes.Where(s => s.ID.ToString() == applyid).ToList()[0];
                sys_overtime_base.ApplyStatus = 1;
                db.Entry<SysOverTime>(sys_overtime_base).State = EntityState.Modified;
                db.SaveChanges();

                //如果是请假则不需要增加可用加班时间，如果是加班需要在个人账户上增加可用加班时间：
                SysUsersRole sys_user = db_user.sur.Find(cook.Values["UserID"]);
                //sys_user.OverTime=list_overtime[0].Time

                return Content("success");
            }
            else if (NowCheckerIndex == sendfors.Length - 2)
            {
                //如果是倒数第二个人审批的此条申请
                //增加一条审批信息，修改nowchecker,并nextchecker设置为0
                sys.NextChecker = 0;
                sys.LastChecker = int.Parse(sendfors[NowCheckerIndex]);
                sys.NowChecker = int.Parse(sendfors[NowCheckerIndex + 1]);
                db_approve.SysApproves.Add(sys);
                db_approve.SaveChanges();
                return Content("success");
            }
            else
            {
                //如果是其他情况，即还有至少两个人在审批流程上
                //增加一条审批信息，修改nowchecker和nextchecker
                sys.LastChecker = int.Parse(sendfors[NowCheckerIndex]);
                sys.NextChecker= int.Parse(sendfors[NowCheckerIndex + 2]);
                sys.NowChecker = int.Parse(sendfors[NowCheckerIndex + 1]);
                db_approve.SysApproves.Add(sys);
                db_approve.SaveChanges();
                return Content("success");

            }

        }



        /// <summary>
        /// 获取审批的通过详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetApproveDetail(int id)
        {
            var list = db_approve.SysApproves.ToList();
            var list_user = db_user.sur.ToList();
            var query = from a in list
                        join b in list_user
                        on a.LastChecker equals b.ID
                        where a.ApplyID == id && a.LastChecker != 0 && a.ApplyType == "加班"
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
            res.Content = result.TrimStart('[').TrimEnd(']');
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
        public ActionResult GetApproveCopy(int row,int page)
        {
            HttpCookie cook = Request.Cookies["userinfo"];

            #region 取出最近的加班的审批进度
            //这段linq是用来将所有最新的审批进度取出来
            var list = db_approve.SysApproves.Where(a => a.ApplyType == "加班");//取出所有进度
            var query = from d in list
                        group d by d.ApplyID into g
                        select new
                        {
                            OpTime = g.Max(x => x.OpTime),
                            ApplyID = g.Key
                        };
            //按不同审批找出不同审批的最大时间，返回审批ID和时间

            var query_final = from b in list
                              join aa in query
                              on new { b.OpTime, b.ApplyID }
                              equals new { aa.OpTime, aa.ApplyID }
                              select b;
            //按照找出的审批ID和时间在所有进度里进行筛选

            var list_end = query_final.ToList();//找出所有审批的最新进度

            var list_overtime = db.SysOverTimes.ToList();

            var query_show = from a in list_end
                             join b in list_overtime
                             on a.ApplyID equals b.ID
                             where (b.CopyFor.Contains("_"+cook.Values["UserID"]+"_"))
                             select new
                             {
                                 b.ID,
                                 b.ProposerID,
                                 b.SendFor,
                                 b.StartTime,
                                 b.EndTime,
                                 b.Time,
                                 b.OverTimeType,
                                 b.Account_Method,
                                 b.OverTimeReason,
                                 b.CopyFor,
                                 a.ApplyID,
                                 a.LastChecker,
                                 a.NextChecker,
                                 a.NowChecker,
                                 a.Applyrate,
                                 a.ApplyStatus
                             };
            #endregion
            var list_show = query_show.ToList();
            //list_show.OrderBy(m=>m.ID)
            var res = new ContentResult();
            string result = JsonTool.LI2J(list_show);
            result = "{\"total\":" + list_show.Count + ",\"rows\":" + result + "}";
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
