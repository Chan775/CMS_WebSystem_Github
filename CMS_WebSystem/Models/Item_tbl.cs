namespace CMS_WebSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item_tbl
    {
        private CMSContext db = new CMSContext();
        [Key]
        [DisplayName("Item ID")]
        public int Itm_Id { get; set; }
        [DisplayName("Customer ID")]
        [Required]
        public int Cust_Id { get; set; }
        [DisplayName("Item Name")]
        [Required]
        [StringLength(50)]
        public string Itm_Name { get; set; }
        [DisplayName("Item Description")]
        [Required]
        [StringLength(100)]
        public string Itm_Description { get; set; }
        [NotMapped]
        [DisplayName("Customer Name")]
        public string Cust_Name
        {
            get
            {
                string cust_Name = null;
                cust_Name = db.Customer_tbl.Find(Cust_Id).Cust_Name;
                return cust_Name;
            }
            set { }
        }
    }
}
