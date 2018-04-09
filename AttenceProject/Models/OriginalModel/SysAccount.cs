using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysAccount
    {
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public float TimeChange { get; set; }

        [Required]
        public string TimeChangeWay { get; set; }

        [Required]
        public float UsableTime { get; set; }

        [MaxLength(100)]
        public string Operator { get; set; }


        public DateTime OpTime { get; set; }
    }
}