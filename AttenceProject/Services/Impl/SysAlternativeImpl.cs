using AttenceProject.Services.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttenceProject.Models;

namespace AttenceProject.Services.Impl
{
    public class SysAlternativeImpl : ISysAlternative
    {
        private SysAlternativeContext db = new SysAlternativeContext();
        public List<SysAlternative> GetSysAlternatives()
        {
            return db.SysAlternatives.ToList();
        }

        public List<SysAlternative> GetSysAlternativesCondition(string AlternativeGroupID, string AlternativeText)
        {
            var list = db.SysAlternatives.ToList();
            if (AlternativeGroupID != "0" && !string.IsNullOrEmpty(AlternativeGroupID))
            {
                list = list.Where(m => m.AlternativeGroupID == int.Parse(AlternativeGroupID)).ToList();
            }
            if (!string.IsNullOrEmpty(AlternativeText))
            {
                list = list.Where(m => m.AlternativeText.Contains(AlternativeText)).ToList();
            }
            return list;

        }
    }
}