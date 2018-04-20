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
    public class SysApplySetImpl : Controller, ISysApplySet
    {
        private SysApplySetContext db = new SysApplySetContext();
        public string GetGroup(int id)
        {
            string result = JsonTool.LI2J(db.SysApplySets.Select(s => new { s.ApplyGroupID, s.ApplyGroupText }).Distinct().ToList());
            return result;
        }

        public string GetApplySetByGroup(int id)
        {
            string result = JsonTool.LI2J(db.SysApplySets.Where(m => m.ApplyGroupID == id).Select(s => new { s.ID, s.ApplyText }).ToList());
            return result;
        }

        public List<SysApplySet> GetSysApplySets()
        {
            return db.SysApplySets.ToList();
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
                SysApplySet SysApplySet = null;
                for (int i = 0; i < id.Split(',').Count(); i++)
                {
                    SysApplySet = db.SysApplySets.Find(int.Parse(id));
                    if (SysApplySet == null)
                    {
                        continue;
                    }
                    db.SysApplySets.Remove(SysApplySet);
                    db.SaveChanges();
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public List<SysApplySet> GetSysApplySetsCondition(string ApplyGroupID, string ApplyText)
        {
            var list = db.SysApplySets.ToList();
            if (ApplyGroupID != "0" && !string.IsNullOrEmpty(ApplyGroupID))
            {
                list = list.Where(m => m.ApplyGroupID == int.Parse(ApplyGroupID)).ToList();
            }
            if (!string.IsNullOrEmpty(ApplyText))
            {
                list = list.Where(m => m.ApplyText.Contains(ApplyText)).ToList();
            }
            return list;

        }

        public SysApplySet GetInfo(int id)
        {
            return db.SysApplySets.Find(id);
        }

        public int SaveEdit(string ID, string ApplyText, string Remarks, string ApplyGroupID)
        {
            IList<SysApplySet> list = db.SysApplySets.Where(m => m.ApplyGroupID.ToString() == ApplyGroupID).ToList();
            SysApplySet sys = db.SysApplySets.Where(a => a.ID.ToString() == ID).ToList()[0];
            if (list.Count > 0)
            {
                string ApplyGroupText = list[0].ApplyGroupText;
                sys.ApplyText = ApplyText;
                sys.ApplyGroupText = ApplyGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.ApplyGroupID = int.Parse(ApplyGroupID);
                db.Entry<SysApplySet>(sys).State = EntityState.Modified;
                return db.SaveChanges();
            }else
            {
                return 0;
            }
        }

        public int SaveAdd(string ApplyText, string Remarks, string ApplyGroupID)
        {
            IList<SysApplySet> list = db.SysApplySets.Where(m => m.ApplyGroupID.ToString() == ApplyGroupID).ToList();
            
                string ApplyGroupText = list[0].ApplyGroupText;

                SysApplySet sys = new SysApplySet();
                sys.ApplyText = ApplyText;
                sys.ApplyGroupText = ApplyGroupText;
                sys.Remarks = Remarks;
                sys.Operator = "admin";
                sys.OpTime = DateTime.Now;
                sys.ApplyGroupID = int.Parse(ApplyGroupID);
                db.SysApplySets.Add(sys);
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