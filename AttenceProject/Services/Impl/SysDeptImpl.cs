using AttenceProject.Services.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttenceProject.Models;
using AttenceProject.App_Start;
using System.Text;
using System.Data.Entity;

namespace AttenceProject.Services.Impl
{
    public class SysDeptImpl : ISysDept
    {
        private SysDeptContext db = new SysDeptContext();
        private SysUsersRoleDbContext db_userrole = new SysUsersRoleDbContext();
        private SysOverTimeContext db_overtime = new SysOverTimeContext();
        private SysVacationContext db_vacation = new SysVacationContext();
        private SysWorkOffContext db_workoff = new SysWorkOffContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();
        public string GetDeptJson()
        {
            string result = JsonTool.LI2J(db.SysDepts.ToList());

            return result;
        }

        public string GetDeptInfo(int id)
        {
            return JsonTool.LI2J(db.SysDepts.Where(m => m.ID == id).ToList());
        }

        public string GetUserInfo(int id)
        {
            var list = db_userrole.sur.ToList();
            var list_alter = db_apply.SysApplySets.ToList();
            var query = from a in list
                        join b in list_alter
                        on a.UserRole equals b.ID
                        where a.ID == id
                        select new
                        {
                            a.ID,
                            a.UserName,
                            UserRole = b.ApplyText,
                            a.UserState
                        };
            string result = JsonTool.EN2J(query.ToList());
            return result;

        }

        public string GetApplySetByGroup(int id)
        {
            return JsonTool.LI2J(db_apply.SysApplySets.Where(m => m.ApplyGroupID == id).Select(s => new { s.ID, s.ApplyText }).ToList());
        }

        public string GetGeneralDeptJson()
        {
            var union1 = db.SysDepts.Select(s => new { ID = s.ID, ParentNode = s.ParentNode, Name = s.DeptName, drag = false, nocheck = true }).ToList();
            var union2 = db_userrole.sur.Select(m => new { ID = m.ID + 10000, ParentNode = m.UserDeptID, Name = m.UserName, drag = true, nocheck = false }).ToList();

            //将dept表与人员表联合查询组成人员部门树，字段名不同的统一名称
            string result = JsonTool.LI2J(union1.Union(union2).ToList());
            return result;
        }

        public int SaveAdd(string DeptName,string ParentNode)
        {
            string CopyFor = "9999";
            string DeptRole = "9999";
            string DeptOrder = "9999";
            SysDept sys = new SysDept();
            sys.DeptName = DeptName;
            sys.DeptOrder = int.Parse(DeptOrder);
            sys.DeptRole = int.Parse(DeptRole);
            sys.CopyFor = CopyFor;
            sys.ParentNode = int.Parse(ParentNode);
            sys.Operator = "admin";
            sys.OpTime = DateTime.Now;
            db.SysDepts.Add(sys);
            return db.SaveChanges();
        }

        public int SaveUserAdd(string ParentNode, string UserName, string LoginName, string UserCode, string PassWord, string UserRole)
        {
            SysUsersRole sys = new SysUsersRole();
            sys.LoginName = LoginName;
            sys.Operator = "admin";
            sys.OpTime = DateTime.Now;
            sys.OverTime = 0;
            sys.PassWord = PassWord;
            sys.UserCode = UserCode;
            sys.UserDeptID = int.Parse(ParentNode);
            sys.UserName = UserName;
            sys.UserRole = int.Parse(UserRole);
            sys.UserState = 1;
            db_userrole.sur.Add(sys);
            return db_userrole.SaveChanges();
        }

        public int SaveDeptEdit(string deptid,string deptname)
        {
            SysDept sys = db.SysDepts.Where(a => a.ID.ToString().Equals(deptid)).ToList()[0];
            sys.DeptName = deptname;
            db.Entry<SysDept>(sys).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /// <summary>
        /// 递归获取某个节点下的所有人员ID
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="staffs"></param>
        /// <returns></returns>
        public IList<int> GetStaff(string deptid, IList<int> staffs)
        {
            var list = db.SysDepts.Where(a => a.ParentNode.ToString().Equals(deptid)).ToList();
            var list_staffs = db_userrole.sur.Where(a => a.UserDeptID.ToString().Equals(deptid)).ToList();
            if (list_staffs.Count > 0)
            {
                for (int i = 0; i < list_staffs.Count; i++)
                {
                    staffs.Add(list_staffs[i].ID);
                }
            }
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string son_deptid = list[i].ID.ToString();
                    staffs = GetStaff(son_deptid, staffs);
                }
            }
            return staffs;
        }

        /// <summary>
        /// 递归获取所有子节点
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="depts"></param>
        /// <returns></returns>
        public IList<int> GetSonDept(string deptid, IList<int> depts)
        {
            var list = db.SysDepts.Where(a => a.ParentNode.ToString().Equals(deptid)).ToList();//取depid的所有子节点
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string son_deptid = list[i].ID.ToString();
                    depts.Add(list[i].ID);
                    depts = GetSonDept(son_deptid, depts);
                }
            }
            return depts;

        }

        public SysDeptStatistical DeptStatic(string deptid, string starttime, string endtime)
        {
            if (string.IsNullOrEmpty(starttime))
            {
                starttime = "0001-01-01";
            }
            if (string.IsNullOrEmpty(endtime))
            {
                endtime = "9999-01-01";
            }
            SysDeptStatistical sds = new SysDeptStatistical();
            IList<int> staffs = new List<int>();
            staffs = GetStaff(deptid, staffs);//员工id的list
            string DeptName = db.SysDepts.Where(a => a.ID.ToString() == deptid).ToList()[0].DeptName;
            sds.DeptName = DeptName;
            sds.CountTime = starttime + "~" + endtime;
            sds.DeptPersonCount = staffs.Count;

            //加班
            var list_overtime = db_overtime.SysOverTimes.ToList();
            var query = from a in list_overtime
                        where (staffs.Contains(a.ProposerID))
                        && a.OpTime > Convert.ToDateTime(starttime) && a.OpTime < Convert.ToDateTime(endtime)
                        select a;
            var list = query.ToList();
            float sum = list.Sum(p => p.Time);
            int count = list.Count();
            sds.OverTimeNum = sum;
            sds.OverTimeCount = count;

            //请假
            var list_vacation = db_vacation.SysVacationsContext.ToList();
            var query_vacation = from a in list_vacation
                                 where (staffs.Contains(a.ProposerID))
                                 && a.OpTime > Convert.ToDateTime(starttime) && a.OpTime < Convert.ToDateTime(endtime)
                                 select a;
            var list2 = query_vacation.ToList();
            float sum_vacation = list2.Sum(p => p.Time);
            int count_vacation = list2.Count();
            sds.VacationNum = sum_vacation;
            sds.VacationCount = count_vacation;

            //调休
            var list_workoff = db_workoff.SysWorkOffs.ToList();
            var query_workoff = from a in list_workoff
                                where (staffs.Contains(a.ProposerID))
                                 && a.OpTime > Convert.ToDateTime(starttime) && a.OpTime < Convert.ToDateTime(endtime)
                                select a;
            var list3 = query_workoff.ToList();
            float sum_workoff = list3.Sum(p => p.Time);
            int count_workoff = list3.Count();
            sds.WorkOffCount = count_workoff;
            sds.WorkOffNum = sum_workoff;

            return sds;
        }

        public string PersonalCount(string username,string applystatus,string type)
        {
            var list_overtime = db_overtime.SysOverTimes.ToList();
            var list_vacation = db_vacation.SysVacationsContext.ToList();
            var list_workoff = db_workoff.SysWorkOffs.ToList();
            var list_user = db_userrole.sur.ToList();
            if (!string.IsNullOrEmpty(username))
            {
                list_user = list_user.Where(a => a.UserName.Contains(username)).ToList();
            }
            if (!string.IsNullOrEmpty(applystatus))
            {
                list_overtime = list_overtime.Where(a => a.ApplyStatus == int.Parse(applystatus)).ToList();
                list_vacation = list_vacation.Where(a => a.ApplyStatus == int.Parse(applystatus)).ToList();
                list_workoff = list_workoff.Where(a => a.ApplyStatus == int.Parse(applystatus)).ToList();
            }
            if (!string.IsNullOrEmpty(type))
            {
                switch (type)
                {
                    case "加班":
                        list_vacation.Clear();
                        list_workoff.Clear();
                        break;
                    case "请假":
                        list_overtime.Clear();
                        list_workoff.Clear();
                        break;
                    case "调休":
                        list_overtime.Clear();
                        list_vacation.Clear();
                        break;
                }
            }
            var list_alter = db_alter.SysAlternatives.ToList();

            #region 取出三种数据
            var query_overtime = from a in list_overtime
                                 join b in list_user
                                 on a.ProposerID equals b.ID
                                 select new
                                 {
                                     a.ID,
                                     a.ProposerID,
                                     a.OpTime,
                                     a.Time,
                                     ApplyStatus = (a.ApplyStatus.ToString() == "0" ? "审批中" : "审批完成"),
                                     a.OverTimeType,
                                     b.UserName,
                                     b.UserCode,
                                     type = "加班"
                                 };
            var list_overtime_join = query_overtime.ToList();
            var query_list1 = from a in list_overtime_join
                              join b in list_alter
                              on a.OverTimeType equals b.ID
                              select new
                              {
                                  a.ID,
                                  a.ProposerID,
                                  a.OpTime,
                                  a.Time,
                                  a.ApplyStatus,
                                  a.UserName,
                                  a.UserCode,
                                  DetailType = b.AlternativeText,
                                  a.type
                              };
            var query_workoff = from a in list_workoff
                                join b in list_user
                                on a.ProposerID equals b.ID
                                select new
                                {
                                    a.ID,
                                    a.ProposerID,
                                    a.OpTime,
                                    a.Time,
                                    ApplyStatus = (a.ApplyStatus.ToString() == "0" ? "审批中" : "审批完成"),
                                    a.WorkOffType,
                                    b.UserName,
                                    b.UserCode,
                                    type = "调休"
                                };
            var list_workoff_join = query_workoff.ToList();
            var query_list2 = from a in list_workoff_join
                              join b in list_alter
                              on a.WorkOffType equals b.ID
                              select new
                              {
                                  a.ID,
                                  a.ProposerID,
                                  a.OpTime,
                                  a.Time,
                                  a.ApplyStatus,
                                  a.UserName,
                                  a.UserCode,
                                  DetailType = b.AlternativeText,
                                  a.type
                              };

            var query_vacation = from a in list_vacation
                                 join b in list_user
                                 on a.ProposerID equals b.ID
                                 select new
                                 {
                                     a.ID,
                                     a.ProposerID,
                                     a.OpTime,
                                     a.Time,
                                     ApplyStatus = (a.ApplyStatus.ToString() == "0" ? "审批中" : "审批完成"),
                                     a.VacationType,
                                     b.UserName,
                                     b.UserCode,
                                     type = "请假"
                                 };
            var list_vacation_join = query_vacation.ToList();
            var query_list3 = from a in list_vacation_join
                              join b in list_alter
                              on a.VacationType equals b.ID
                              select new
                              {
                                  a.ID,
                                  a.ProposerID,
                                  a.OpTime,
                                  a.Time,
                                  a.ApplyStatus,
                                  a.UserName,
                                  a.UserCode,
                                  DetailType = b.AlternativeText,
                                  a.type
                              };
            #endregion

            var list1 = query_list1.ToList();
            var list2 = query_list2.ToList();
            var list3 = query_list3.ToList();
            int total = list1.Count + list2.Count + list3.Count;
            string result = JsonTool.LI2J(list1.Union(list2).Union(list3).ToList());
            return "{\"total\":" + total + ",\"rows\":" + result + "}"; ;
        }

        public int DeletePerson(string userid)
        {
            SysUsersRole sysuserrole = db_userrole.sur.Find(int.Parse(userid));
            db_userrole.sur.Remove(sysuserrole);
            return db_userrole.SaveChanges();
        }

        public int DeleteDept(string deptid)
        {
            SysDept sysdept = db.SysDepts.Find(int.Parse(deptid));
            db.SysDepts.Remove(sysdept);
            return db.SaveChanges();
        }

    }

}