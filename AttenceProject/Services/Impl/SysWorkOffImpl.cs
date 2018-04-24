using AttenceProject.Services.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttenceProject.Models;
using AttenceProject.App_Start;
using System.Data.Entity;
using System.Collections;

namespace AttenceProject.Services.Impl
{
    public class SysWorkOffImpl : ISysWorkOff
    {
        private SysVacationContext db = new SysVacationContext();
        private SysOverTimeContext db_overtime = new SysOverTimeContext();
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

        public int SaveAdd(HttpCookie cook, string VacationReason, string VacationType, string Emergency, string StartTime, string EndTime, string SendFor, string CopyFor)
        {
            string userid = cook.Values["UserID"];
            string username = cook.Values["UserName"];
            string usercode = cook.Values["UserCode"];
            try
            {
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

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IList<SysVacation> GetList(string userid)
        {
            var list=db.SysVacationsContext.Where(m => m.ProposerID.ToString() == userid).ToList();
            return list;
        }
        public IList<SysVacation> GetCopyList(string userid)
        {
            var list = db.SysVacationsContext.Where(m => m.CopyFor.Contains("_" + userid + "_")).ToList();
            return list;
        }

        public string GetApprove(HttpCookie cook)
        {
            string userid = cook.Values["UserID"];
            #region 取出最近的加班的审批进度
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
            #endregion

            string result = JsonTool.LI2J(query_show.ToList());
            result = "{\"total\":" + list_end.Count + ",\"rows\":" + result + "}";
            return result;
        }
        public string GetVacationInfo(int id)
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
            return result;
        }


        public int SaveApprove(HttpCookie cook, string applyrate, string applystatus, string applyid)
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
                db.Entry<SysVacation>(sys_vacation_base).State = EntityState.Modified;
                db.SaveChanges();
                //如果是请假则需要在个人账户上减少时间，如果是加班需要在个人账户上增加可用加班时间

                return 1;
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
                return 1;
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
                return 1;

            }
        }

        public string GetApproveDetail(int id)
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
            return result;
        }

        public string GetApproveCopy(HttpCookie cook)
        {
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

            var list_overtime = db.SysVacationsContext.ToList();

            var query_show = from a in list_end
                             join b in list_overtime
                             on a.ApplyID equals b.ID
                             where (b.CopyFor.Contains("_" + cook.Values["UserID"] + "_"))
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
            string result = JsonTool.LI2J(list_show);
            result = "{\"total\":" + list_show.Count + ",\"rows\":" + result + "}";
            return result;
        }


    }
}