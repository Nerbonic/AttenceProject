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

    }
}
