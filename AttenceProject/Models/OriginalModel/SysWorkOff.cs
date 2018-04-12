using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace AttenceProject.Models
{

    public partial class SysWorkOff
    {
        public int ID { get; set; }

        [Required]
        public int ProposerID { get; set; }

        [Required]
        public int WorkOffType { get; set; }

        [Required]
        public DateTime OverTimeStart { get; set; }

        [Required]
        public DateTime OverTimeEnd { get; set; }

        [Required]
        public DateTime VacationStart { get; set; }

        [Required]
        public DateTime VacationEnd { get; set; }

        [Required]
        public float Time { get; set; }

        [Required]
        [MaxLength(2000)]
        public string VacationReason { get; set; }

        [Required]
        [MaxLength(50)]
        public string Emergency { get; set; }

        [MaxLength(100)]
        public string SendFor { get; set; }

        [MaxLength(100)]
        public string CopyFor { get; set; }

        public int ApplyStatus { get; set; }

        public DateTime OpTime { get; set; }
    }
}
