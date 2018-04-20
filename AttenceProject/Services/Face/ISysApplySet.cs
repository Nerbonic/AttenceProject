using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttenceProject.Services.Face
{
    public interface ISysApplySet
    {
        List<SysApplySet> GetSysApplySets();

        List<SysApplySet> GetSysApplySetsCondition(string ApplyGroupID, string ApplyText);

        string Delete(string ids);

        SysApplySet GetInfo(int id);

        int SaveEdit(string ID, string ApplyText, string Remarks, string ApplyGroupID);

        int SaveAdd(string ApplyText, string Remarks, string ApplyGroupID);

        string GetGroup(int id);

        string GetApplySetByGroup(int id);
    }
}