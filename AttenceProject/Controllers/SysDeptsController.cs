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
    public class SysDeptsController : Controller
    {
        private SysDeptContext db = new SysDeptContext();

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

    }
}
