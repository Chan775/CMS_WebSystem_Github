using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS_WebSystem.Models;
using PagedList;

namespace CMS_WebSystem.Controllers
{
    public class BookingController : Controller
    {
        private CMSContext db = new CMSContext();

        public ActionResult ViewBooking(string sortOrder, string searchString, string currentFilter, int? page)
        {

            List<Booking_tbl> bookingList = null;
            if (Session["userType"].ToString() == "Agent")
            {
                bookingList = db.Booking_tbl.ToList();
                bookingList = bookingList.Where(a => a.Agent_Id.ToString() == Session["userId"].ToString()).ToList();
            }
            else
            {
                bookingList = db.Booking_tbl.ToList();
            }

            var bookingDetailList = new List<BookingInfo>();

            foreach (var booking in bookingList)
            {
                var bookinginfo = new BookingInfo(booking);
                bookingDetailList.Add(bookinginfo);
            }
            IEnumerable<BookingInfo> bookingDetailList2 = bookingDetailList as IEnumerable<BookingInfo>;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.BkDateSortParm = String.IsNullOrEmpty(sortOrder) ? "BkDate_desc" : "";
            ViewBag.ScheduleIdSortParm = sortOrder == "schId" ? "schId_desc" : "schId";
            ViewBag.DeptLocationSortParm = sortOrder == "deptLocation" ? "deptLocation_desc" : "deptLocation";
            ViewBag.ArrvlLocationSortParm = sortOrder == "arrvlLocation" ? "arrvlLocation_desc" : "arrvlLocation";
            ViewBag.DeptDateSortParm = sortOrder == "deptDate" ? "deptDate_desc" : "deptDate";
            ViewBag.ArrvlDateSortParm = sortOrder == "arrvlDate" ? "arrvlDate_desc" : "arrvlDate";
            ViewBag.AgentNameSortParm = sortOrder == "agentName" ? "agentName_desc" : "agentName";
            ViewBag.BkIdSortParm = sortOrder == "bkId" ? "bkId_desc" : "bkId";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                bookingDetailList2 = bookingDetailList2.Where(s => s.agent.Name.Contains(searchString) || s.schedule.Departure_Location.Contains(searchString)
                || s.schedule.Arrival_Location.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "BkDate_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.booking.Bk_Date);
                    break;
                case "schId":
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.schedule.Sch_Id);
                    break;
                case "schId_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.schedule.Sch_Id);
                    break;
                case "deptLocation":
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.schedule.Departure_Location);
                    break;
                case "deptLocation_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.schedule.Departure_Location);
                    break;
                case "arrvlLocation":
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.schedule.Arrival_Location);
                    break;
                case "arrvlLocation_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.schedule.Arrival_Location);
                    break;
                case "deptDate":
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.schedule.Dept_DateTime);
                    break;
                case "deptDate_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.schedule.Dept_DateTime);
                    break;
                case "arrvlDate":
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.schedule.Arrvl_DateTime);
                    break;
                case "arrvlDate_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.schedule.Arrvl_DateTime);
                    break;
                case "agentName":
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.agent.Name);
                    break;
                case "agentName_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.agent.Name);
                    break;
                case "bkId":
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.booking.Bk_Id);
                    break;
                case "bkId_desc":
                    bookingDetailList2 = bookingDetailList2.OrderByDescending(s => s.booking.Bk_Id);
                    break;
                default:
                    bookingDetailList2 = bookingDetailList2.OrderBy(s => s.booking.Bk_Date);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(bookingDetailList2.ToPagedList(pageNumber, pageSize));

        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking_tbl booking_tbl = db.Booking_tbl.Find(id);
            var bookinginfo = new BookingInfo(booking_tbl);
            if (booking_tbl == null)
            {
                return HttpNotFound();
            }
            return View(bookinginfo);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Create()
        {
            List<BookingItem_tbl> ci = new List<BookingItem_tbl>();
            ViewBag.ContainerCount = getContainerNumber().ToList();
            ViewBag.ScheduleIdList = getScheduleId().ToList();
            ViewBag.ItemList = getItemList().ToList();
            return View(ci.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult Create(List<BookingItem_tbl> items, string schId)
        {

            int n;
            bool result = int.TryParse(schId, out n);
            if (result == false)
            {             
                TempData["message"] = "Please Select a schedule";
                
                return Json(Url.Action("Create", "Booking"));
            }
            else if (items == null)
            {               
                TempData["message"] = "Please add an item";
                return Json(Url.Action("Create", "Booking"));
            }
            else if (items != null)
            {
                bool checkItem = true;
                //string message = null;
                int itemCount = 0;
                foreach (var item in items.ToList())
                {
                    if (item.Itm_Id.ToString() == "" || item.Container_No.ToString() == "")
                    {
                        checkItem = false;
                        break;
                    }
                }

                if (checkItem == true)
                {
                    int scheduleId = Convert.ToInt32(schId);
                    Schedule_tbl targetSchedule = db.Schedule_tbl.Find(scheduleId);
                    foreach (var item in items)
                    {
                        itemCount = itemCount + item.Container_No;
                    }
                    if (targetSchedule.Available_Container >= itemCount)
                    {
                        Booking_tbl newBooking = new Booking_tbl();
                        newBooking.Bk_Date = DateTime.Now;
                        newBooking.Agent_Id = Convert.ToInt32(Session["userId"].ToString());
                        newBooking.Sch_Id = scheduleId;
                        db.Booking_tbl.Add(newBooking);
                        db.SaveChanges();

                        foreach (var item in items.ToList())
                        {
                            BookingItem_tbl newBI = new BookingItem_tbl();
                            newBI.Itm_Id = item.Itm_Id;
                            newBI.Container_No = item.Container_No;
                            newBI.Bk_Id = newBooking.Bk_Id;
                            db.BookingItem_tbl.Add(newBI);
                            db.SaveChanges();
                        }
                      

                        targetSchedule.Available_Container = targetSchedule.Available_Container - itemCount;
                        db.Entry(targetSchedule).State = EntityState.Modified;
                        db.SaveChanges();

                        //message = "New Booking Created!";
                        //return Json(message);
                        TempData["message"] = "New Booking Created!";
                        return Json(Url.Action("ViewBooking", "Booking"));

                    }
                    else
                    {
                        //message = "Not enought container for the items!";
                        //return Json(message);
                        TempData["message"] = "Not enought container for the items!";
                        return Json(Url.Action("Create", "Booking"));

                    }


                }
                else
                {
                    //message = "Please make sure all items are added properly!";
                    //return Json(message);
                    TempData["message"] = "Please make sure all items are added properly!";
                    return Json(Url.Action("Create", "Booking"));
                }

            }

            return Json(Url.Action("Create", "Booking"));



        }

        public List<SelectListItem> getScheduleId()
        {
            var scheduleIdList = new List<SelectListItem>();
            foreach (var schedule in db.Schedule_tbl.ToList())
            {
                if (schedule.Sch_Status == "Waiting")
                {
                    scheduleIdList.Add(new SelectListItem() { Text = schedule.Sch_Id.ToString(), Value = schedule.Sch_Id.ToString() });
                }

            }
            return scheduleIdList;
        }
        public List<SelectListItem> getItemList()
        {
            var itemList = new List<SelectListItem>();
            foreach (var item in db.Item_tbl.ToList())
            {
                string custName = db.Customer_tbl.Find(item.Cust_Id).Cust_Name;
                itemList.Add(new SelectListItem() { Text = item.Itm_Name.ToString() + "-" + custName, Value = item.Itm_Id.ToString() });
            }
            return itemList;
        }
        public List<SelectListItem> getContainerNumber()
        {
            var count = new List<SelectListItem>();
            count.Add(new SelectListItem() { Text = "1", Value = "1" });
            count.Add(new SelectListItem() { Text = "2", Value = "2" });
            count.Add(new SelectListItem() { Text = "3", Value = "3" });
            count.Add(new SelectListItem() { Text = "4", Value = "4" });
            count.Add(new SelectListItem() { Text = "5", Value = "5" });
            count.Add(new SelectListItem() { Text = "6", Value = "6" });
            count.Add(new SelectListItem() { Text = "7", Value = "7" });
            count.Add(new SelectListItem() { Text = "8", Value = "8" });
            count.Add(new SelectListItem() { Text = "9", Value = "9" });
            return count;
        }
        public ActionResult GetScheduleInfo(string scheduleId)
        {
            int n;
            bool result = Int32.TryParse(scheduleId, out n);
            if (result == true)
            {
                int schId = Convert.ToInt32(scheduleId);
                Schedule_tbl schTable = db.Schedule_tbl.Find(schId);
                List<string> schInfo = new List<string>();
                schInfo.Add(schTable.Departure_Location);
                schInfo.Add(schTable.Arrival_Location);
                schInfo.Add(schTable.Dept_DateTime.ToString());
                schInfo.Add(schTable.Arrvl_DateTime.ToString());
                schInfo.Add(schTable.Ship_Name);
                schInfo.Add(schTable.Available_Container.ToString());
                return Json(schInfo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> schInfo = new List<string>();
                schInfo.Add("");
                schInfo.Add("");
                schInfo.Add("");
                schInfo.Add("");
                schInfo.Add("");
                schInfo.Add("");
                return Json(schInfo, JsonRequestBehavior.AllowGet);
            }


        }
    }
}
