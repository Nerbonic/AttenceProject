using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttenceProject.Services.Face
{
    public interface ILogin
    {
        IList<SysUsersRole> GetUserInfoByName(string username);

    }
}