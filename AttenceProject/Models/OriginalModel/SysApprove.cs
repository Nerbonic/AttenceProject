using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysApprove
    {
        public int ID { get; set; }

        [Required]
        public int ApplyID { get; set; }

        [Required]
        public int ApplyStatus { get; set; }

        [Required]
        public string ApplyType { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Applyrate { get; set; }

        public int LastChecker { get; set; }

        [Required]
        public int NowChecker { get; set; }


        public int NextChecker { get; set; }


        public DateTime OpTime { get; set; }
    }
}