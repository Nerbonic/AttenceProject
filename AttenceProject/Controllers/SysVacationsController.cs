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
            string result = JsonTool.LI2J(db_alter.SysAlternatives.Where(m => m.AlternativeGroupText == "请假类型").ToList());
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
            string result = JsonTool.LI2J(db_apply.SysApplySets.Where(m => m.ApplyGroupText == "申请时间点设置").ToList());
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
            string userid = cook.Values["UserID"];
            string username = cook.Values["UserName"];
            string usercode = cook.Values["UserCode"];

            #region 新建请假信息
            SysVacation sys = new SysVacation();
            sys.VacationReason = VacationReason;
            sys.ProposerID = int.Parse(userid);
            sys.VacationType = int.Parse(VacationType);
            sys.Emergency = Emergency;

            sys.ApplyStatus = 0;
            sys.StartTime = DateTime.Parse(StartTime);
            sys.EndTime = DateTime.Parse(EndTime);
            sys.CopyFor = "11";
            string[] SendFors = SendFor.TrimEnd('_').Split('_');
            foreach (var sendfor in SendFors)
            {
                sys.SendFor += (int.Parse(sendfor) - 10000).ToString() + '_';
            }
            TimeSpan ts1 = sys.EndTime - sys.StartTime;//计算请假总经历时间的时间戳
            int hours = ts1.Hours - 9;//计算请假结束当天的多出来的请假时间
            int days = ts1.Days;//若请假大于1天，计算请假天数
            if (days == 0)
            {
                hours = ts1.Hours;
            }
            sys.Time = days + hours / 24;//计算出总时长，形式：4.2代表4天+0.2天，即4天+4.8小时
            sys.OpTime = DateTime.Now;
            db.SysVacationsContext.Add(sys);
            db.SaveChanges();

            #endregion

            #region 新建审批流信息
            SysApprove sysapprove = new SysApprove();
            sysapprove.ApplyType = "请假";
            sysapprove.ApplyID = sys.ID;
            sysapprove.LastChecker = 0;
            sysapprove.ApplyStatus = 0;
            sysapprove.OpTime = DateTime.Now;
            sysapprove.NextChecker = 0;
            if (sys.SendFor.TrimEnd('_').Split('_').Length > 1)
            {
                sysapprove.NextChecker = int.Parse(sys.SendFor.Split('_')[1]);
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

            var list = db.SysVacationsContext.ToList();
            //if (AlternativeGroupID != "0" && !string.IsNullOrEmpty(AlternativeGroupID))
            //{
            //    list = list.Where(m => m.AlternativeGroupID == int.Parse(AlternativeGroupID)).ToList();
            //}
            //if (!string.IsNullOrEmpty(AlternativeText))
            //{
            //    list = list.Where(m => m.AlternativeText.Contains(AlternativeText)).ToList();
            //}
            string result = JsonTool.LI2J(list);
            result = "{\"total\":" + db.SysVacationsContext.ToList().Count + ",\"rows\":" + result + "}";
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
        public ActionResult GetApprove()
        {
            HttpCookie cook = Request.Cookies["userinfo"];

            //取审批时，从所有最新审批中找到符合以下两个条件的审批进度：
            //1、审批尚未完成
            //2、最新进度的NowChecker为登录人的

            //这段linq是用来将所有最新的【请假】审批进度取出来
            var list = db_approve.SysApproves.Where(a => a.ApplyType == "请假");//取出所有进度
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

            var list_vacation = db.SysVacationsContext.ToList();

            var query_show = from a in list_end
                             join b in list_vacation
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
                                 b.VacationType,
                                 b.VacationReason,
                                 b.Emergency,
                                 b.CopyFor,
                                 a.ApplyID,
                                 a.LastChecker,
                                 a.NextChecker,
                                 a.NowChecker,
                                 a.Applyrate,
                                 a.ApplyStatus
                             };

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
        public ActionResult GetVacationInfo(int id)
        {
            var list1 = db.SysVacationsContext.Where(m => m.ID == id).ToList();

            var list2 = db_user.sur.ToList();

            var query = from a in list1
                        join b in list2
                        on a.ProposerID equals b.ID
                        select new
                        {
                            a.ID,
                            b.UserName,
                            a.ProposerID,
                            a.StartTime,
                            a.EndTime,
                            a.Time,
                            a.VacationType,
                            a.VacationReason,
                            a.Emergency
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

        public ActionResult SaveApprove(string applyrate, string applystatus, string applyid)
        {
            IList<SysApprove> list = db_approve.SysApproves.Where(m => m.ApplyID.ToString() == applyid).ToList();
            IList<SysVacation> list_vacation = db.SysVacationsContext.Where(s => s.ID.ToString() == applyid).ToList();
            //获取请假信息及最近一次的审批信息

            string sendfor = list_vacation[0].SendFor;
            string[] sendfors = sendfor.TrimEnd('_').Split('_');
            IList sendlist = sendfors.ToList();

            SysApprove sys = new SysApprove();
            SysApprove sys_first = list[0];
            sys.ApplyID = int.Parse(applyid);
            sys.Applyrate = applyrate;
            sys.ApplyStatus = int.Parse(applystatus);
            sys.OpTime = DateTime.Now;
            sys.ApplyType = "请假";
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

                //结束一个审批流程
                //结束审批流程要做的事情：修改申请信息为通过:
                SysVacation sys_vacation_base = db.SysVacationsContext.Where(s => s.ID.ToString() == applyid).ToList()[0];
                sys_vacation_base.ApplyStatus = 1;
                db.Entry<SysVacation>(sys_vacation_base).State= EntityState.Modified;
                db.SaveChanges();
                //如果是请假则需要在个人账户上减少时间，如果是加班需要在个人账户上增加可用加班时间

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
                sys.NextChecker = int.Parse(sendfors[NowCheckerIndex + 2]);
                sys.NowChecker = int.Parse(sendfors[NowCheckerIndex + 1]);
                db_approve.SysApproves.Add(sys);
                db_approve.SaveChanges();
                return Content("success");

            }

        }

        public ActionResult GetApproveDetail(int id)
        {
            string result = JsonTool.LI2J(db_approve.SysApproves.Where(m => m.ApplyID == id && m.LastChecker != 0 && m.ApplyType == "请假").ToList());
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
    }
}
