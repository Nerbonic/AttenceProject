using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysVacation
    {   
        public int ID { get; set; }

        [Required]
        public int ProposerID { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public float Time { get; set; }

        [Required]
        public int VacationType { get; set; }

        [Required]
        public string VacationReason { get; set; }

        [Required]
        public string CopyFor { get; set; }

        [Required]
        public string SendFor { get; set; }

        public int ApplyStatus { get; set; }

        public string Emergency { get; set; }

        public DateTime OpTime { get; set; }
    }
}