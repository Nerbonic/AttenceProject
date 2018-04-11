using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysUsersRole
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserCode { get; set; }

        public int UserDeptID { get; set; }

        [Required]
        [MaxLength(100)]
        public string LoginName { get; set; }

        [Required]
        [MaxLength(100)]
        public string PassWord { get; set; }

        public int UserState { get; set; }

        public int UserRole { get; set; }

        public int OverTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Operator { get; set; }

        public DateTime OpTime { get; set; }
    }
}