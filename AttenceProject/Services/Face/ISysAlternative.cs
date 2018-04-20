using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttenceProject.Services.Face
{
    public interface ISysAlternative
    {
        List<SysAlternative> GetSysAlternatives();

        List<SysAlternative> GetSysAlternativesCondition(string AlternativeGroupID, string AlternativeText);

        string Delete(string ids);

        SysAlternative GetInfo(int id);

        int SaveEdit(string ID, string AlternativeText, string Remarks, string AlternativeGroupID);

        int SaveAdd(string AlternativeText, string Remarks, string AlternativeGroupID);

        string GetGroup(int id);

        string GetAlternativbeByGroup(int id);
    }
}