namespace CMS_WebSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookingItem_tbl
    {
        private CMSContext db = new CMSContext();

        [Key]
        public int BI_Id { get; set; }
        public int Bk_Id { get; set; }
        public int Itm_Id { get; set; }
        [Required]
        [DisplayName("Container No.")]
        [Range(1, 9)]
        public int Container_No { get; set; }

        [NotMapped]
        [DisplayName("Item Name")]
        public string Item_Name
        {
            get
            {

                string name = null;
                name = db.Item_tbl.Find(Itm_Id).Itm_Name;
                return name;
            }
            set { }
        }
        [NotMapped]
        [DisplayName("Item Description")]
        public string Item_Description
        {
            get
            {

                string name = null;
                name = db.Item_tbl.Find(Itm_Id).Itm_Description;
                return name;
            }
            set { }
        }
    }
}
