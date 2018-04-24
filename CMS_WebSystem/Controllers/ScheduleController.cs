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
    public class ScheduleController : Controller
    {
        private CMSContext db = new CMSContext();

        // GET: Schedule
        public ActionResult ScheduleManagement(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ScheduleIdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.DeptLocationSortParm = sortOrder == "deptLocation" ? "deptLocation_desc" : "deptLocation";
            ViewBag.ArrvlLocationSortParm = sortOrder == "arrvlLocation" ? "arrvlLocation_desc" : "arrvlLocation";
            ViewBag.DeptDateSortParm = sortOrder == "deptDate" ? "deptDate_desc" : "deptDate";
            ViewBag.ArrvlDateSortParm = sortOrder == "arrvlDate" ? "arrvlDate_desc" : "arrvlDate";
            ViewBag.ContainerSortParm = sortOrder == "container" ? "container_desc" : "container";
            var ScheduleList = db.Schedule_tbl.Where(a => a.Sch_Id.ToString() != null);

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
                ScheduleList = ScheduleList.Where(s => s.Sch_Id.ToString().Contains(searchString) || s.Departure_Location.ToString().Contains(searchString) || s.Arrival_Location.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_desc":
                    ScheduleList = ScheduleList.OrderByDescending(s => s.Sch_Id);
                    break;
                case "deptLocation":
                    ScheduleList = ScheduleList.OrderBy(s => s.Departure_Location);
                    break;
                case "deptLocation_desc":
                    ScheduleList = ScheduleList.OrderByDescending(s => s.Departure_Location);
                    break;
                case "arrvlLocation":
                    ScheduleList = ScheduleList.OrderBy(s => s.Arrival_Location);
                    break;
                case "arrvlLocation_desc":
                    ScheduleList = ScheduleList.OrderByDescending(s => s.Arrival_Location);
                    break;
                case "deptDate":
                    ScheduleList = ScheduleList.OrderBy(s => s.Dept_DateTime);
                    break;
                case "deptDate_desc":
                    ScheduleList = ScheduleList.OrderByDescending(s => s.Dept_DateTime);
                    break;
                case "arrvlDate":
                    ScheduleList = ScheduleList.OrderBy(s => s.Arrvl_DateTime);
                    break;
                case "arrvlDate_desc":
                    ScheduleList = ScheduleList.OrderByDescending(s => s.Arrvl_DateTime);
                    break;
                case "container":
                    ScheduleList = ScheduleList.OrderBy(s => s.Available_Container);
                    break;
                case "container_desc":
                    ScheduleList = ScheduleList.OrderByDescending(s => s.Available_Container);
                    break;
                default:
                    ScheduleList = ScheduleList.OrderBy(s => s.Sch_Id);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(ScheduleList.ToPagedList(pageNumber, pageSize));
        }

        // GET: Schedule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule_tbl schedule_tbl = db.Schedule_tbl.Find(id);
            if (schedule_tbl == null)
            {
                return HttpNotFound();
            }
            return View(schedule_tbl);
        }

        // GET: Schedule/Create
        public ActionResult Create()
        {
            ViewBag.PortLocation = getLocation();
            ViewBag.ShipList = getShips();
            return View();
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sch_Id,Departure_Location,Arrival_Location,Dept_DateTime,Arrvl_DateTime,Ship_Id,Available_Container")] Schedule_tbl schedule_tbl)
        {
            if (ModelState.IsValid)
            {
                if (checkSchedule(schedule_tbl) == true)
                {
                    schedule_tbl.Available_Container = db.Ship_tbl.Find(schedule_tbl.Ship_Id).Container_Limit;
                    db.Schedule_tbl.Add(schedule_tbl);
                    db.SaveChanges();
                    TempData["message"] = "New Schedule Registered!";
                    return RedirectToAction("ScheduleManagement", "Schedule");
                }
                else
                {
                    ViewBag.PortLocation = getLocation();
                    ViewBag.ShipList = getShips();
                    return View();
                }

            }

            return View(schedule_tbl);
        }

        // GET: Schedule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule_tbl schedule_tbl = db.Schedule_tbl.Find(id);
            ViewBag.PortLocation = getLocation();
            ViewBag.ShipList = getShips();
            if (db.Booking_tbl.Where(a => a.Sch_Id == schedule_tbl.Sch_Id).Count() > 0)
            {
                TempData["message"] = "This schedule cannot be edit, it has been booked!";
                return RedirectToAction("ScheduleManagement");
            }
            if (schedule_tbl == null)
            {
                return HttpNotFound();
            }

            return View(schedule_tbl);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sch_Id,Departure_Location,Arrival_Location,Dept_DateTime,Arrvl_DateTime,Ship_Id")] Schedule_tbl schedule_tbl)
        {
            if (ModelState.IsValid)
            {
                if (checkSchedule(schedule_tbl) == true)
                {
                    Schedule_tbl targetSchedule = db.Schedule_tbl.Find(schedule_tbl.Sch_Id);
                    targetSchedule.Arrival_Location = schedule_tbl.Arrival_Location;
                    targetSchedule.Arrvl_DateTime = schedule_tbl.Arrvl_DateTime;
                    targetSchedule.Departure_Location = schedule_tbl.Departure_Location;
                    targetSchedule.Dept_DateTime = schedule_tbl.Dept_DateTime;
                    targetSchedule.Ship_Id = schedule_tbl.Ship_Id;
                    targetSchedule.Available_Container = db.Ship_tbl.Find(targetSchedule.Ship_Id).Container_Limit;
                    db.Entry(targetSchedule).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Schedule has been edited!";
                    return RedirectToAction("ScheduleManagement");
                }
                else
                {
                    ViewBag.PortLocation = getLocation();
                    ViewBag.ShipList = getShips();
                    return View();
                }

            }
            return View(schedule_tbl);
        }

        // GET: Schedule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule_tbl schedule_tbl = db.Schedule_tbl.Find(id);
            if (db.Booking_tbl.Where(a => a.Sch_Id == schedule_tbl.Sch_Id).Count() > 0)
            {
                TempData["message"] = "This schedule cannot be delete, it has been booked!";
                return RedirectToAction("ScheduleManagement");
            }
            if (schedule_tbl == null)
            {
                return HttpNotFound();
            }
            return View(schedule_tbl);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule_tbl schedule_tbl = db.Schedule_tbl.Find(id);
            db.Schedule_tbl.Remove(schedule_tbl);
            db.SaveChanges();
            TempData["message"] = "Schedule Deleted!";
            return RedirectToAction("ScheduleManagement");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<SelectListItem> getLocation()
        {
            var locationList = new List<SelectListItem>();
            locationList.Add(new SelectListItem() { Text = "Kuching Port", Value = "Kuching Port" });
            locationList.Add(new SelectListItem() { Text = "Menumbok Jetty", Value = "Menumbok Jetty" });
            locationList.Add(new SelectListItem() { Text = "Tabaco", Value = "Tabaco" });
            locationList.Add(new SelectListItem() { Text = "Busan", Value = "Busan" });
            locationList.Add(new SelectListItem() { Text = "Hong Kong", Value = "Hong Kong" });
            locationList.Add(new SelectListItem() { Text = "Osaka", Value = "Osaka" });
            locationList.Add(new SelectListItem() { Text = "Labuan", Value = "Labuan" });
            locationList.Add(new SelectListItem() { Text = "Shanghai", Value = "Shanghai" });
            locationList.Add(new SelectListItem() { Text = "Rason", Value = "Rason" });
            locationList.Add(new SelectListItem() { Text = "Jinzhou", Value = "Jinzhou" });
            return locationList;
        }
        public List<SelectListItem> getShips()
        {
            var shipList = new List<SelectListItem>();
            foreach (var ship in db.Ship_tbl)
            {
                shipList.Add(new SelectListItem() { Text = ship.Ship_Name, Value = ship.Ship_Id.ToString() });
            }
            return shipList;
        }

        public Boolean checkSchedule(Schedule_tbl sch_input)
        {
            Boolean check = true;
            var schList = db.Schedule_tbl.Where(a => a.Ship_Id == sch_input.Ship_Id && a.Sch_Id != sch_input.Sch_Id);
            if (sch_input.Departure_Location == sch_input.Arrival_Location)
            {
                TempData["message"] = "Departure and Arrival Location must be different!";
                check = false;

            }
            else if (sch_input.Dept_DateTime > sch_input.Arrvl_DateTime)
            {
                TempData["message"] = "Departure date must be earier than arrival date!";
                check = false;
            }
            else if (sch_input.Dept_DateTime < DateTime.Now)
            {
                TempData["message"] = "Please select future departure date!";
                check = false;
            }
            if (schList != null)
            {
                foreach (var schedule in schList)
                {
                    if ((sch_input.Dept_DateTime >= schedule.Dept_DateTime && sch_input.Dept_DateTime <= schedule.Arrvl_DateTime) ||
                       (sch_input.Arrvl_DateTime >= schedule.Dept_DateTime && sch_input.Arrvl_DateTime <= schedule.Arrvl_DateTime))
                    {
                        TempData["message"] = "The vessel is reserved by other schdule on the selected period!";
                        check = false;
                    }
                }
            }

            return check;
        }
    }
}
