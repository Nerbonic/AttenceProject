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
using AttenceProject.Services.Impl;
using AttenceProject.Services.Face;

namespace AttenceProject.Controllers
{
    public class SysDeptsController : Controller
    {
        private SysDeptContext db = new SysDeptContext();
        private SysUsersRoleDbContext db_userrole = new SysUsersRoleDbContext();
        private SysOverTimeContext db_overtime = new SysOverTimeContext();
        private SysVacationContext db_vacation = new SysVacationContext();
        private SysWorkOffContext db_workoff = new SysWorkOffContext();
        private SysAlternativeContext db_alter = new SysAlternativeContext();
        private SysApplySetContext db_apply = new SysApplySetContext();

        public ISysDept service_dept = new SysDeptImpl();

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
            string result = service_dept.GetDeptJson();
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
            string result = service_dept.GetDeptInfo(id);
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
            string result = service_dept.GetUserInfo(id);
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            StringBuilder sbnew = new StringBuilder();
            return Content(sb.ToString().TrimStart('[').TrimEnd(']'));
        }

        /// <summary>
        /// 获取申请设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult GetApplySetByGroup(int id)
        {
            string result = service_dept.GetApplySetByGroup(id);
            var res = new ContentResult();
            res.Content = result;
            res.ContentType = "application/json";
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }


        /// <summary>
        /// 获取人员+部门树的json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGeneralDeptJson()
        {
            string result = service_dept.GetGeneralDeptJson();
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
            service_dept.SaveAdd(DeptName, ParentNode);
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
            string md5 = JsonTool.GetMd5Hash(PassWord);
            service_dept.SaveUserAdd(ParentNode, UserName, LoginName, UserCode, md5, UserRole);
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
            service_dept.SaveDeptEdit(deptid, deptname);
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

            SysDeptStatistical sds = service_dept.DeptStatic(deptid, starttime, endtime);
            var res = new ContentResult();
            res.Content = JsonTool.EN2J(sds);
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

       

        public ActionResult PersonalStatistical()
        {
            return View();
        }

        /// <summary>
        /// 获取个人统计列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalCount(string username,string applystatus,string type)
        {
            var res = new ContentResult();
            string result = service_dept.PersonalCount(username, applystatus, type);
            StringBuilder sb = new StringBuilder();
            sb.Append(result);
            res.Content = result;
            res.ContentType = "application/json";
            //res.Data = sb.ToString();
            res.ContentEncoding = Encoding.UTF8;
            return res;
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult DeletePerson(string userid)
        {
            if (string.IsNullOrEmpty(userid))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_dept.DeletePerson(userid);
            return Content("success");
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public ActionResult DeleteDept(string deptid)
        {
            if (string.IsNullOrEmpty(deptid))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_dept.DeleteDept(deptid);
            return Content("success");
        }

        /// <summary>
        /// 删除部门及员工
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public ActionResult DeleteAll(string deptid)
        {
            IList<int> staffs = new List<int>();
            IList<int> depts = new List<int>();
            staffs = service_dept.GetStaff(deptid, staffs);//员工id的list
            depts = service_dept.GetSonDept(deptid, depts);//子部门id的list
            for (int i = 0; i < staffs.Count; i++)
            {
                service_dept.DeletePerson(staffs[i].ToString());
            }
            for(int i = 0; i < depts.Count; i++)
            {
                service_dept.DeleteDept(depts[i].ToString());
            }
            service_dept.DeleteDept(deptid);
            return Content("success");
        }
    }
}
