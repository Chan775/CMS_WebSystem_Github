namespace CMS_WebSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CMSContext : DbContext
    {
        public CMSContext()
            : base("name=CMSContext")
        {
        }

        public virtual DbSet<Booking_tbl> Booking_tbl { get; set; }
        public virtual DbSet<BookingItem_tbl> BookingItem_tbl { get; set; }
        public virtual DbSet<Customer_tbl> Customer_tbl { get; set; }
        public virtual DbSet<Item_tbl> Item_tbl { get; set; }
        public virtual DbSet<Schedule_tbl> Schedule_tbl { get; set; }
        public virtual DbSet<Ship_tbl> Ship_tbl { get; set; }
        public virtual DbSet<User_tbl> User_tbl { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
