using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebSystem.Models
{
    public class BookingInfo
    {
        private CMSContext db = new CMSContext();

        public Booking_tbl booking { get; set; }

        public Schedule_tbl schedule { get; set; }

        public User_tbl agent { get; set; }

        public IEnumerable<BookingItem_tbl> bookingItem { get; set; }
        public BookingInfo()
        {

        }

        public BookingInfo(Booking_tbl bk)
        {
            setBookingInfo(bk);
        }

        public void setBookingInfo(Booking_tbl bk)
        {
            booking = bk;
            schedule = db.Schedule_tbl.Find(bk.Sch_Id);
            bookingItem = db.BookingItem_tbl.Where(a => a.Bk_Id == bk.Bk_Id);
            agent = db.User_tbl.Find(bk.Agent_Id);

        }
    }
}