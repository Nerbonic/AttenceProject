using AttenceProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection.Emit;

namespace AttenceProject
{
    public class SysUsersRoleDbContext : DbContext
    {
        public SysUsersRoleDbContext()
             : base("name=connStr")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<SysUsersRole> sur { get; set; }
    }
}
