namespace CMS_WebSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ship_tbl
    {
        [Key]
        [DisplayName("Vessel ID")]
        public int Ship_Id { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Vessel Name")]
        public string Ship_Name { get; set; }
        [Required]
        [DisplayName("Container Limit")]
        public int Container_Limit { get; set; }
    }
}
