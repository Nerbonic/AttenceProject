using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttenceProject.Models
{
    public class SysDeptStatistical
    {
        public string DeptName { get; set; }

        public int DeptPersonCount { get; set; }

        public string CountTime { get; set; }

        public int OverTimeCount { get; set; }

        public float OverTimeNum { get; set; }

        public int VacationCount { get; set; }

        public float VacationNum { get; set; }

        public int WorkOffCount { get; set; }

        public float WorkOffNum { get; set; }

    }
}