using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection.Emit;

namespace EFCodeFirst
{
    public class CodeFirstDBContext : DbContext
    {
       public CodeFirstDBContext()
            :base("name=connStr")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<StudentInfo> StudentInfo { get; set; }
    }
}
