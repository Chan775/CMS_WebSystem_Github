namespace CMS_WebSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_tbl
    {
        [Key]
        [DisplayName("User ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [StringLength(10, MinimumLength = 4, ErrorMessage = "password cannot be longer than 10 characters and less than 4 characters")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("User Type")]
        public string Type { get; set; }
        [Required]
        [Range(100000000, 999999999, ErrorMessage = "Please enter 9 digits of contact number")]
        [DisplayName("Contact Number")]
        public int Contact { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
    }
}
