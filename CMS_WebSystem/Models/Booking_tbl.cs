namespace CMS_WebSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Booking_tbl
    {
        [Key]
        public int Bk_Id { get; set; }
        public int Sch_Id { get; set; }
        [DisplayName("Booking Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Bk_Date { get; set; }
        public int Agent_Id { get; set; }
    }
}
