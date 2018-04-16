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

namespace AttenceProject.Controllers
{
    public class SysDeptsController : Controller
    {
        private SysDeptContext db = new SysDeptContext();
        private View_GeneralTreeContext db_GeneralTree = new View_GeneralTreeContext();
        private SysUsersRoleDbContext db_userrole = new SysUsersRoleDbContext();
        private SysOverTimeContext db_overtime = new SysOverTimeContext();
        private SysVacationContext db_vacation = new SysVacationContext();
        private SysWorkOffContext db_workoff = new SysWorkOffContext();


        // GET: SysDepts
        public ActionResult Index()
        {
            return View();
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
        /// 获取部门树json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeptJson()
        {
            string result = JsonTool.LI2J(db.SysDepts.ToList());
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            StringBuilder sbnew = new StringBuilder();

            sbnew = sb.Replace("{\"ID\":", "{\"id\":").Replace(",\"DeptName\":", ",\"name\":").Replace(",\"ParentNode\":", ",\"pId\":");
            
            return Content(sbnew.ToString().TrimStart('{').TrimEnd('}'));
        }
        /// <summary>
        /// 获取指定部门的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDeptInfo(int id)
        {
            string result = JsonTool.LI2J(db.SysDepts.Where(m=>m.ID==id).ToList());
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            StringBuilder sbnew = new StringBuilder();
            return Content(sb.ToString().TrimStart('[').TrimEnd(']'));
        }

        /// <summary>
        /// 获取指定人员的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetUserInfo(int id)
        {
            string result = JsonTool.LI2J(db_userrole.sur.Where(m => m.ID == id).ToList());
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            StringBuilder sbnew = new StringBuilder();
            return Content(sb.ToString().TrimStart('[').TrimEnd(']'));
        }
        /// <summary>
        /// 获取人员+部门树的json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGeneralDeptJson()
        {
            var union1 = db.SysDepts.Select(s => new { ID = s.ID, ParentNode = s.ParentNode, Name = s.DeptName,drag=false, nocheck=true }).ToList();
            var union2= db_userrole.sur.Select(m => new { ID = m.ID + 10000, ParentNode = m.UserDeptID, Name = m.UserName,drag=true,nocheck=false}).ToList();
            
            //将dept表与人员表联合查询组成人员部门树，字段名不同的统一名称
            string result = JsonTool.LI2J(union1.Union(union2).ToList());
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            StringBuilder sbnew = new StringBuilder();

            sbnew = sb.Replace("{\"ID\":", "{\"id\":").Replace(",\"Name\":", ",\"name\":").Replace(",\"ParentNode\":", ",\"pId\":");

            return Content(sbnew.ToString().TrimStart('{').TrimEnd('}'));
        }
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="DeptName">部门名称</param>
        /// <param name="CopyFor">部门抄送人</param>
        /// <param name="ParentNode">父级节点</param>
        /// <returns></returns>
        public ActionResult SaveAdd(string DeptName,string ParentNode)
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
            db.SaveChanges();
            return Content("success");
        }
        /// <summary>
        /// 保存新增用户
        /// </summary>
        /// <param name="ParentNode"></param>
        /// <param name="UserName"></param>
        /// <param name="LoginName"></param>
        /// <param name="UserCode"></param>
        /// <param name="PassWord"></param>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public ActionResult SaveUserAdd(string ParentNode,string UserName,string LoginName,string UserCode,string PassWord,string UserRole)
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
            db_userrole.SaveChanges();
            return Content("success");

        }

        /// <summary>
        /// 保存部门的修改
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptname"></param>
        /// <returns></returns>
        public ActionResult SaveDeptEdit(string deptid,string deptname)
        {
            SysDept sys = db.SysDepts.Where(a => a.ID.ToString().Equals(deptid)).ToList()[0];
            sys.DeptName = deptname;
            db.Entry<SysDept>(sys).State = EntityState.Modified;
            db.SaveChanges();
            return Content("success");
        }
        public ActionResult DeptStatistical()
        {
            return View();
        }

        /// <summary>
        /// 获取一级部门分组列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDeptsGroup(int id)
        {
            string result = JsonTool.LI2J(db.SysDepts.Where(a => a.ParentNode == 16).ToList());
            if (id == 0)
            {
                result = "[{\"ID\":16,\"DeptName\":\"全体部门\"}," + result.TrimStart('[');
            }
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 获取部门统计信息
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns>时间段内申请的总的加班时长_申请次数</returns>
        public ActionResult DeptStatic(string deptid,string starttime,string endtime)
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
            float sum= list.Sum(p => p.Time);
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

            var res = new ContentResult();
            res.Content = JsonTool.EN2J(sds);
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 获取某个节点下的所有人员ID
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="staffs"></param>
        /// <returns></returns>
        public IList<int> GetStaff(string deptid,IList<int> staffs)
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
                for(int i = 0; i < list.Count; i++)
                {
                    string son_deptid = list[i].ID.ToString();
                    staffs = GetStaff(son_deptid, staffs);
                }
            }
            return staffs;
        }

        public ActionResult PersonalStatistical()
        {

        }

    }
}
