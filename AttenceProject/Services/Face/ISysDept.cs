using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttenceProject.Services.Face
{
    public interface ISysDept
    {
        string GetDeptJson();

        string GetDeptInfo(int id);

        string GetUserInfo(int id);

        string GetApplySetByGroup(int id);

        string GetGeneralDeptJson();

        int SaveAdd(string DeptName, string ParentNode);

        int SaveUserAdd(string ParentNode, string UserName, string LoginName, string UserCode, string PassWord, string UserRole);

        int SaveDeptEdit(string depid, string deptname);

        SysDeptStatistical DeptStatic(string deptid, string starttime, string endtime);

        string PersonalCount(string username, string applystatus, string type);

        int DeletePerson(string userid);

        int DeleteDept(string deptid);

        IList<int> GetStaff(string deptid, IList<int> staffs);

        IList<int> GetSonDept(string deptid, IList<int> depts);
    }
}