using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeFirstDBContext db = new CodeFirstDBContext();
            db.Database.CreateIfNotExists();
            StudentInfo si = new StudentInfo();
            si.Id = 1;
            si.StuName = "dafsfda";
            si.SubTime = DateTime.Now;
            db.StudentInfo.Add(si);
            db.SaveChanges();
        }
    }

}
