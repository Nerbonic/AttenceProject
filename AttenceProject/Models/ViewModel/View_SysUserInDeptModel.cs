namespace AttenceProject.Models.ViewModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class View_SysUserInDeptModel : DbContext
    {
        public View_SysUserInDeptModel()
            : base("name=connStr")
        {
        }

        public virtual DbSet<View_SysUserInDeptModel> View_SysUserInDeptModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
