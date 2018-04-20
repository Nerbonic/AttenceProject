using AttenceProject.Services.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttenceProject.Models;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using AttenceProject.App_Start;

namespace AttenceProject.Services.Impl
{
    public class SysAlternativeImpl : Controller,ISysAlternative
    {
        private SysAlternativeContext db = new SysAlternativeContext();
        public string GetGroup(int id)
        {
            string result = JsonTool.LI2J(db.SysAlternatives.Select(s => new { s.AlternativeGroupID, s.AlternativeGroupText }).Distinct().ToList());
            return result;
        }

        public string GetAlternativbeByGroup(int id)
        {
            string result = JsonTool.LI2J(db.SysAlternatives.Where(m => m.AlternativeGroupID == id).Select(s => new { s.ID, s.AlternativeText }).ToList());
            return result;
        }

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

        public SysAlternative GetInfo(int id)
        {
            return db.SysAlternatives.Find(id);
        }

        public int SaveEdit(string ID, string AlternativeText, string Remarks, string AlternativeGroupID)
        {
            IList<SysAlternative> list = db.SysAlternatives.Where(m => m.AlternativeGroupID.ToString() == AlternativeGroupID).ToList();
            SysAlternative sys = db.SysAlternatives.Where(a => a.ID.ToString() == ID).ToList()[0];
            if (list.Count > 0)
            {
                string AlternativeGroupText = list[0].AlternativeGroupText;
                sys.AlternativeText = AlternativeText;
                sys.AlternativeGroupText = AlternativeGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.AlternativeGroupID = int.Parse(AlternativeGroupID);
                db.Entry<SysAlternative>(sys).State = EntityState.Modified;
                return db.SaveChanges();
            }else
            {
                return 0;
            }
        }

        public int SaveAdd(string AlternativeText, string Remarks, string AlternativeGroupID)
        {
            IList<SysAlternative> list = db.SysAlternatives.Where(m => m.AlternativeGroupID.ToString() == AlternativeGroupID).ToList();
            
                string AlternativeGroupText = list[0].AlternativeGroupText;

                SysAlternative sys = new SysAlternative();
                sys.AlternativeText = AlternativeText;
                sys.AlternativeGroupText = AlternativeGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.AlternativeGroupID = int.Parse(AlternativeGroupID);
                db.SysAlternatives.Add(sys);
                return db.SaveChanges();
            
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