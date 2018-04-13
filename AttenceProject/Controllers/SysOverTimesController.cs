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

namespace AttenceProject.Controllers
{
    public class SysOverTimesController : BaseController
    {
        private SysOverTimeContext db = new SysOverTimeContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();
        private SysUsersRoleDbContext db_user = new SysUsersRoleDbContext();
        private SysApproveContext db_approve = new SysApproveContext();
        private View_SysUserInDeptContext db_ViewUserInDept = new View_SysUserInDeptContext();
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
        public ActionResult SaveAdd(string OverTimeReason, string OverTimeType, string Account_Method, string StartTime, string EndTime,string SendFor,string CopyFor)
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
            sys.CopyFor = CopyFor;
            string[] SendFors= SendFor.TrimEnd('_').Split('_');
            string[] CopyFors = SendFor.TrimEnd('_').Split('_');
            foreach (var sendfor in SendFors)
            {
                sys.SendFor += (int.Parse(sendfor) - 10000).ToString() + '_';
            }
            foreach (var copyfor in CopyFors)
            {
                sys.CopyFor += (int.Parse(copyfor) - 10000).ToString() + '_';
            }
            sys.CopyFor = '_' + sys.CopyFor;
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
            sysapprove.ApplyType = "加班";
            sysapprove.ApplyID = sys.ID;
            sysapprove.LastChecker = 0;
            sysapprove.ApplyStatus = 0;
            sysapprove.OpTime = DateTime.Now;
            sysapprove.NextChecker = 0;
            if (sys.SendFor.TrimEnd('_').Split('_').Length > 1)
            {
                sysapprove.NextChecker= int.Parse(sys.SendFor.Split('_')[1]);
            }
            sysapprove.NowChecker = int.Parse(sys.SendFor.Split('_')[0]);
            sysapprove.Applyrate = "申请发起";
            db_approve.SysApproves.Add(sysapprove);
            db_approve.SaveChanges();
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
            return View();
        }

        public ActionResult ApproveCopy()
        {
            return View();
        }
        public ActionResult GetApprove()
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
                        where b.ApplyStatus != 1 && a.NowChecker == int.Parse(cook.Values["UserID"])
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

            var res = new ContentResult();
            string result = JsonTool.LI2J(query_show.ToList());
            result = "{\"total\":" + list_end.Count + ",\"rows\":" + result + "}";
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }
        public ActionResult GetOverTimeInfo(int id)
        {
            var list1 = db.SysOverTimes.Where(m => m.ID == id).ToList() ;

            var list2 = db_user.sur.ToList();

            var query = from a in list1
                        join b in list2
                        on a.ProposerID equals b.ID
                        select new {
                            a.ID,
                            b.UserName,
                            a.ProposerID,
                            a.StartTime,
                            a.EndTime,
                            a.Time,
                            a.OverTimeType,
                            a.OverTimeReason,
                            a.Account_Method
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

        public ActionResult GetApproveDetail(int id)
        {
            string result = JsonTool.LI2J(db_approve.SysApproves.Where(m => m.ApplyID == id && m.LastChecker != 0 && m.ApplyType == "加班").ToList());
            if (string.IsNullOrEmpty(result))
            {
                return HttpNotFound();
            }
            var res = new ContentResult();
            res.Content = result.TrimStart('{').TrimEnd('}');
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
