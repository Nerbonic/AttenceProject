namespace AttenceProject.Models.ViewModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class View_GeneralTreeContext : DbContext
    {
        public View_GeneralTreeContext()
            : base("name=connStr")
        {
        }

        public virtual DbSet<View_GeneralTreeContext> View_GeneralTrees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
