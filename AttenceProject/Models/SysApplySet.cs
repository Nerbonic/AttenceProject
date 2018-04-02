using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysApplySet
    {   
        public int ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ApplyText { get; set; }

        [Required]
        public int ApplyGroupID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ApplyGroupText { get; set; }

        [MaxLength(2000)]
        public string Remarks { get; set; }

        [MaxLength(500)]
        public string Operator { get; set; }

        public DateTime OpTime { get; set; }
    }
}