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
    public class AgentController : Controller
    {
        private CMSContext db = new CMSContext();

        // GET: Agent
        public ActionResult AgentManagement(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ContactSortParm = sortOrder == "contact" ? "contact_desc" : "contact";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            var AgentList = db.User_tbl.Where(a => a.Type == "Agent");

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
                AgentList = AgentList.Where(s => s.Name.Contains(searchString) || s.EmailAddress.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    AgentList = AgentList.OrderByDescending(s => s.Name);
                    break;
                case "contact":
                    AgentList = AgentList.OrderBy(s => s.Contact);
                    break;
                case "contact_desc":
                    AgentList = AgentList.OrderByDescending(s => s.Contact);
                    break;
                case "email":
                    AgentList = AgentList.OrderBy(s => s.EmailAddress);
                    break;
                case "email_desc":
                    AgentList = AgentList.OrderByDescending(s => s.EmailAddress);
                    break;

                default:
                    AgentList = AgentList.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(AgentList.ToPagedList(pageNumber, pageSize));
        }

        // GET: Agent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_tbl user_tbl = db.User_tbl.Find(id);
            if (user_tbl == null)
            {
                return HttpNotFound();
            }
            return View(user_tbl);
        }

        // GET: Agent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Password,Type,Contact,EmailAddress")] User_tbl user_tbl)
        {
            if (ModelState.IsValid)
            {

                if (db.User_tbl.Where(a => a.Name == user_tbl.Name).Count() > 0)
                {
                    TempData["message"] = "This name has been registered!";
                    return View();
                }
                else if (db.User_tbl.Where(a => a.EmailAddress == user_tbl.EmailAddress).Count() > 0)
                {
                    TempData["message"] = "This email address has been registered!";
                    return View();
                }
                else
                {
                    user_tbl.Type = "Agent";
                    db.User_tbl.Add(user_tbl);
                    db.SaveChanges();
                    TempData["message"] = "New Agent Registered!";
                    return RedirectToAction("AgentManagement", "Agent");
                }


            }

            return View(user_tbl);
        }

        // GET: Agent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_tbl user_tbl = db.User_tbl.Find(id);

            if (user_tbl == null)
            {
                return HttpNotFound();
            }
            return View(user_tbl);
        }

        // POST: Agent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Password,Type,Contact,EmailAddress")] User_tbl user_tbl)
        {
            if (ModelState.IsValid)
            {
                User_tbl targetUser = db.User_tbl.Find(user_tbl.Id);
                if (db.User_tbl.Where(a => a.EmailAddress == user_tbl.EmailAddress && a.Id != user_tbl.Id).Count() > 0)
                {
                    TempData["message"] = "This email has been registered!";
                    return View(user_tbl.Id);

                }
                if (targetUser.Id.ToString() == Session["userId"].ToString())
                {
                    targetUser.Password = user_tbl.Password;
                }
                targetUser.Name = user_tbl.Name;
                targetUser.Contact = user_tbl.Contact;
                targetUser.EmailAddress = user_tbl.EmailAddress;
                db.Entry(targetUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Edit Successful!";
                return RedirectToAction("AgentManagement", "Agent");

            }
            return View(user_tbl);
        }

        // GET: Agent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_tbl user_tbl = db.User_tbl.Find(id);
            if (user_tbl == null)
            {
                return HttpNotFound();
            }
            return View(user_tbl);
        }

        // POST: Agent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_tbl user_tbl = db.User_tbl.Find(id);
            if (db.Booking_tbl.Where(a => a.Agent_Id == id).Count() == 0)
            {
                db.User_tbl.Remove(user_tbl);
                db.SaveChanges();
                TempData["message"] = "Agent Deleted!";
                return RedirectToAction("AgentManagement");
            }
            else
            {
                TempData["message"] = "Cannot delete agent! This agent is associated with some booking";
                return RedirectToAction("Delete", "Agent", new { id = user_tbl.Id });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}