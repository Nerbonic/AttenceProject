namespace AttenceProject.Models.ViewModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class View_SysUserInDeptContext : DbContext
    {
        public View_SysUserInDeptContext()
            : base("name=connStr")
        {
        }

        public virtual DbSet<View_SysUserInDeptContext> View_SysUserInDepts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
