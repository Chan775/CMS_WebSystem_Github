namespace CMS_WebSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer_tbl
    {
        [Key]
        [DisplayName("Customer ID")]
        public int Cust_Id { get; set; }
        [DisplayName("Name")]
        [Required]
        [StringLength(100)]
        public string Cust_Name { get; set; }
        [DisplayName("Contact Number")]
        [Required]
        [Range(100000000, 999999999, ErrorMessage = "Please enter 9 digits of contact number")]
        public int Cust_Contact { get; set; }
        [DisplayName("Email Address")]
        [Required]
        [StringLength(100)]
        public string Cust_EmailAddress { get; set; }
        [DisplayName("Address")]
        [Required]
        [StringLength(200)]
        public string Cust_Address { get; set; }
    }
}
