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
    public class CustomerController : Controller
    {
        private CMSContext db = new CMSContext();

        // GET: Customer
        public ActionResult CustomerManagement(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ContactSortParm = sortOrder == "contact" ? "contact_desc" : "contact";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.AddressSortParm = sortOrder == "address" ? "address_desc" : "address";
            var CustomerList = db.Customer_tbl.Where(a => a.Cust_Id.ToString() != null);

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
                CustomerList = CustomerList.Where(s => s.Cust_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    CustomerList = CustomerList.OrderByDescending(s => s.Cust_Name);
                    break;
                case "contact":
                    CustomerList = CustomerList.OrderBy(s => s.Cust_Contact);
                    break;
                case "contact_desc":
                    CustomerList = CustomerList.OrderByDescending(s => s.Cust_Contact);
                    break;
                case "email":
                    CustomerList = CustomerList.OrderBy(s => s.Cust_EmailAddress);
                    break;
                case "email_desc":
                    CustomerList = CustomerList.OrderByDescending(s => s.Cust_EmailAddress);
                    break;
                case "address":
                    CustomerList = CustomerList.OrderBy(s => s.Cust_Address);
                    break;
                case "address_desc":
                    CustomerList = CustomerList.OrderByDescending(s => s.Cust_Address);
                    break;

                default:
                    CustomerList = CustomerList.OrderBy(s => s.Cust_Name);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(CustomerList.ToPagedList(pageNumber, pageSize));
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_tbl customer_tbl = db.Customer_tbl.Find(id);
            if (customer_tbl == null)
            {
                return HttpNotFound();
            }
            return View(customer_tbl);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cust_Id,Cust_Name,Cust_Contact,Cust_EmailAddress,Cust_Address")] Customer_tbl customer_tbl)
        {
            if (ModelState.IsValid)
            {
                if (db.Customer_tbl.Where(a => a.Cust_Name == customer_tbl.Cust_Name).Count() > 0)
                {
                    TempData["message"] = "This name has been registered!";
                    return View();
                }
                else if (db.Customer_tbl.Where(a => a.Cust_EmailAddress == customer_tbl.Cust_EmailAddress).Count() > 0)
                {
                    TempData["message"] = "This email address has been registered!";
                    return View();
                }
                else
                {
                    db.Customer_tbl.Add(customer_tbl);
                    db.SaveChanges();
                    TempData["message"] = "New Customer Registered!";
                    return RedirectToAction("CustomerManagement");

                }

            }

            return View(customer_tbl);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_tbl customer_tbl = db.Customer_tbl.Find(id);
            if (customer_tbl == null)
            {
                return HttpNotFound();
            }
            return View(customer_tbl);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cust_Id,Cust_Name,Cust_Contact,Cust_EmailAddress,Cust_Address")] Customer_tbl customer_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer_tbl).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Edit successful!";
                return RedirectToAction("CustomerManagement");
            }
            return View(customer_tbl);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_tbl customer_tbl = db.Customer_tbl.Find(id);
            if (customer_tbl == null)
            {
                return HttpNotFound();
            }
            return View(customer_tbl);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer_tbl customer_tbl = db.Customer_tbl.Find(id);
            db.Customer_tbl.Remove(customer_tbl);
            db.SaveChanges();
            TempData["message"] = "Delete Successful!";
            return RedirectToAction("CustomerManagement");
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
