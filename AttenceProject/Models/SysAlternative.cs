using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysAlternative
    {   
        public int ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string AlternativeText { get; set; }

        public int AlternativeGroupID { get; set; }

        [Required]
        [MaxLength(200)]
        public string AlternativeGroupText { get; set; }

        [MaxLength(2000)]
        public string Remarks { get; set; }

        [MaxLength(500)]
        public string Operator { get; set; }

        public DateTime OpTime { get; set; }
    }
}