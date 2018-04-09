namespace AttenceProject.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_SysUserInDept
    {
        [StringLength(500)]
        public string CopyFor { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptRole { get; set; }

        [Column(Order = 1)]
        [StringLength(100)]
        public string DeptName { get; set; }

        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Column(Order = 3)]
        [StringLength(100)]
        public string UserName { get; set; }

        [Column(Order = 4)]
        [StringLength(100)]
        public string UserCode { get; set; }

        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserDeptID { get; set; }

        [Column(Order = 6)]
        [StringLength(100)]
        public string LoginName { get; set; }

        [Column(Order = 7)]
        [StringLength(1000)]
        public string PassWord { get; set; }

        public int UserState { get; set; }

        public int OverTime { get; set; }
    }
}
