using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace EFCodeFirst
{
    public class StudentInfo
    {
        [Key]
        public int Id { get; set; }
        [StringLength(32)]
        [Required]
        public string StuName { get; set; }
        [Required]
        public DateTime SubTime { get; set; }   
    }
}
