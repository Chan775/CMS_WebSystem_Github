namespace CMS_WebSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Schedule_tbl
    {
        private CMSContext db = new CMSContext();

        [Key]
        [DisplayName("Schedule ID")]
        public int Sch_Id { get; set; }
        [DisplayName("Departure Location")]
        [Required]
        [StringLength(50)]
        public string Departure_Location { get; set; }
        [DisplayName("Arrival Location")]
        [Required]
        [StringLength(50)]
        public string Arrival_Location { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Departure Date")]
        public System.DateTime Dept_DateTime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Arrival Date")]
        public System.DateTime Arrvl_DateTime { get; set; }
        [Required]
        [DisplayName("Vessel ID")]
        public int Ship_Id { get; set; }

        [DisplayName("Available Container")]
        public int Available_Container { get; set; }

        [NotMapped]
        [DisplayName("Vessel Name")]
        public string Ship_Name
        {
            get
            {
                string shipName = null;
                shipName = db.Ship_tbl.Find(Ship_Id).Ship_Name;
                return shipName;
            }


            set { }
        }
        [NotMapped]
        [DisplayName("Status")]
        public string Sch_Status
        {
            get
            {
                string currentStatus = null;
                DateTime currentDate = DateTime.Now;
                if (Dept_DateTime > currentDate)
                {
                    currentStatus = "Waiting";
                }
                else if (Dept_DateTime < currentDate && Arrvl_DateTime > currentDate)
                {
                    currentStatus = "Delivering";
                }
                else if (currentDate > Arrvl_DateTime)
                {
                    currentStatus = "Completed";
                }
                return currentStatus;
            }
            set { }
        }
    }
}
