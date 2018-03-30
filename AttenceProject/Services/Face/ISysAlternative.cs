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
    }
}