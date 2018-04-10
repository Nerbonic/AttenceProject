namespace AttenceProject.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_GeneralTree
    {

        [Key]
        public int ID { get; set; }

        public int ParentNode { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

       
    }
}
