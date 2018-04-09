using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysDept
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string DeptName { get; set; }

        public int ParentNode { get; set; }

        public int DeptOrder { get; set; }

        public int DeptRole { get; set; }

        [MaxLength(500)]
        public string CopyFor { get; set; }

        [MaxLength(100)]
        public string Operator { get; set; }


        public DateTime OpTime { get; set; }
    }
}