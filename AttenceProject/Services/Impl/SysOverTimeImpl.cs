using AttenceProject.Services.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttenceProject.Models;
using AttenceProject.App_Start;

namespace AttenceProject.Services.Impl
{
    public class SysOverImpl : ISysOverTime
    {
        private SysOverTimeContext db = new SysOverTimeContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();
        private SysUsersRoleDbContext db_user = new SysUsersRoleDbContext();
        private SysApproveContext db_approve = new SysApproveContext();

        public string GetAlternativesData(string groupname)
        {
            string result = JsonTool.LI2J(db_alter.SysAlternatives.Where(m => m.AlternativeGroupText == groupname).ToList());
            return result;
        }
        public string GetTimePoint(string groupname)
        {
            string result = JsonTool.LI2J(db_apply.SysApplySets.Where(m => m.ApplyGroupText == groupname).ToList());
            return result;
        }

        public int SaveAdd(HttpCookie cook, string OverTimeReason, string OverTimeType, string Account_Method, string StartTime, string EndTime, string SendFor, string CopyFor)
        {
            string userid = cook.Values["UserID"];
            string username = cook.Values["UserName"];
            string usercode = cook.Values["UserCode"];
            try
            {
                #region 新建加班信息
                SysOverTime sys = new SysOverTime();
                sys.OverTimeReason = OverTimeReason;
                sys.ProposerID = int.Parse(userid);
                sys.OverTimeType = int.Parse(OverTimeType);
                sys.Account_Method = int.Parse(Account_Method);
                sys.ApplyStatus = 0;
                sys.StartTime = DateTime.Parse(StartTime);
                sys.EndTime = DateTime.Parse(EndTime);
                string[] SendFors = SendFor.TrimEnd('_').Split('_');
                string[] CopyFors = CopyFor.TrimEnd('_').Split('_');
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
                    sysapprove.NextChecker = int.Parse(sys.SendFor.Split('_')[1]);
                }
                sysapprove.NowChecker = int.Parse(sys.SendFor.Split('_')[0]);
                sysapprove.Applyrate = "申请发起";
                db_approve.SysApproves.Add(sysapprove);
                db_approve.SaveChanges();
                #endregion

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IList<SysOverTime> GetList(string userid)
        {
            var list=db.SysOverTimes.Where(m => m.ProposerID.ToString() == userid).ToList();
            return list;
        }
        public IList<SysOverTime> GetCopyList(string userid)
        {
            var list = db.SysOverTimes.Where(m => m.CopyFor.Contains("_" + userid + "_")).ToList();
            return list;
        }

        public string GetApprove(HttpCookie cook)
        {
            string userid = cook.Values["UserID"];
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
                             where b.ApplyStatus != 1 && a.NowChecker == int.Parse(userid)
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

            string result = JsonTool.LI2J(query_show.ToList());
            result = "{\"total\":" + list_end.Count + ",\"rows\":" + result + "}";
            return result;
        }
        public string GetOverTimeInfo(int id)
        {
            var list1 = db.SysOverTimes.Where(m => m.ID == id).ToList();

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
                            a.OverTimeType,
                            a.OverTimeReason,
                            a.Account_Method
                        };
            string result = JsonTool.LI2J(query.ToList());
            return result;
        }

        
    }
}