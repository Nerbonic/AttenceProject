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

namespace AttenceProject.Controllers
{
    public class SysDeptsController : Controller
    {
        private SysDeptContext db = new SysDeptContext();
        private View_GeneralTreeContext db_GeneralTree = new View_GeneralTreeContext();
        private SysUsersRoleDbContext db_userrole = new SysUsersRoleDbContext();

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
            var union1 = db.SysDepts.Select(s => new { ID = s.ID, ParentNode = s.ParentNode, Name = s.DeptName,drag=false }).ToList();
            var union2= db_userrole.sur.Select(m => new { ID = m.ID + 10000, ParentNode = m.UserDeptID, Name = m.UserName,drag=true}).ToList();
            
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

    }
}
