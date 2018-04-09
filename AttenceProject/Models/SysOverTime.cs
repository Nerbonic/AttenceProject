using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysOverTime
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
        public int OverTimeType { get; set; }

        [Required]
        public int Account_Method { get; set; }

        [Required]
        public string OverTimeReason { get; set; }

        [Required]
        public string CopyFor { get; set; }

        [Required]
        public int ApplyStatus { get; set; }

        public DateTime OpTime { get; set; }
    }
}