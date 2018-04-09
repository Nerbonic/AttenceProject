using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysMenu
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string MenuText { get; set; }

        public int MenuParentID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Path { get; set; }

        [Required]
        [MaxLength(500)]
        public string Operator { get; set; }

        public DateTime OpTime { get; set; }
    }
}