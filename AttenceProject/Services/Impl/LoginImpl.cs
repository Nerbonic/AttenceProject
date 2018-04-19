using AttenceProject.Services.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttenceProject.Models;
using AttenceProject.App_Start;

namespace AttenceProject.Services.Impl
{
    public class LoginImpl : ILogin
    {
        private SysUsersRoleDbContext db = new SysUsersRoleDbContext();
        public IList<SysUsersRole> GetUserInfoByName(string username)
        {
            IList<SysUsersRole> list = db.sur.Where(m => m.LoginName == username).ToList();
            return list;
        }
    }
}