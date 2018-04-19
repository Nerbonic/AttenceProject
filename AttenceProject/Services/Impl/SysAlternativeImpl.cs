using AttenceProject.Services.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttenceProject.Models;
using System.Web.Mvc;
using System.Net;

namespace AttenceProject.Services.Impl
{
    public class SysAlternativeImpl : Controller,ISysAlternative
    {
        private SysAlternativeContext db = new SysAlternativeContext();
        public List<SysAlternative> GetSysAlternatives()
        {
            return db.SysAlternatives.ToList();
        }

        public string Delete(string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    return "fail";
                }
                string id = ids.TrimStart('[').TrimEnd(']');
                SysAlternative sysAlternative = null;
                for (int i = 0; i < id.Split(',').Count(); i++)
                {
                    sysAlternative = db.SysAlternatives.Find(int.Parse(id));
                    if (sysAlternative == null)
                    {
                        continue;
                    }
                    db.SysAlternatives.Remove(sysAlternative);
                    db.SaveChanges();
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}