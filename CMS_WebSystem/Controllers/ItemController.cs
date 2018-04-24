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
    public class ItemController : Controller
    {
        private CMSContext db = new CMSContext();

        // GET: Item
        public ActionResult ItemManagement(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DescriptionSortParm = sortOrder == "description" ? "description_desc" : "description";
            var ItemList = db.Item_tbl.Where(a => a.Itm_Id.ToString() != null);

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
                ItemList = ItemList.Where(s => s.Itm_Name.Contains(searchString) || s.Itm_Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    ItemList = ItemList.OrderByDescending(s => s.Itm_Name);
                    break;
                case "description":
                    ItemList = ItemList.OrderBy(s => s.Itm_Description);
                    break;
                case "description_desc":
                    ItemList = ItemList.OrderByDescending(s => s.Itm_Description);
                    break;

                default:
                    ItemList = ItemList.OrderBy(s => s.Itm_Name);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(ItemList.ToPagedList(pageNumber, pageSize));
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_tbl item_tbl = db.Item_tbl.Find(id);
            if (item_tbl == null)
            {
                return HttpNotFound();
            }
            return View(item_tbl);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.custList = getCustomer();
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Itm_Id,Cust_Id,Itm_Name,Itm_Description")] Item_tbl item_tbl)
        {
            if (ModelState.IsValid)
            {
                if (db.Item_tbl.Where(a => a.Itm_Name == item_tbl.Itm_Name && a.Cust_Id == item_tbl.Cust_Id).Count() > 0)
                {
                    TempData["message"] = "The customer already have item with same name!";
                    ViewBag.custList = getCustomer();
                    return View();
                }
                else
                {
                    db.Item_tbl.Add(item_tbl);
                    db.SaveChanges();
                    TempData["message"] = "New item registered!";
                    return RedirectToAction("ItemManagement");
                }

            }

            return View(item_tbl);
        }


        // GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_tbl item_tbl = db.Item_tbl.Find(id);
            if (db.BookingItem_tbl.Where(a => a.Itm_Id == item_tbl.Itm_Id).Count() > 0)
            {
                TempData["message"] = "Item cannot be delete as it associated with some booking!";
                return View();
            }
            if (item_tbl == null)
            {
                return HttpNotFound();
            }
            return View(item_tbl);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item_tbl item_tbl = db.Item_tbl.Find(id);
            db.Item_tbl.Remove(item_tbl);
            db.SaveChanges();
            TempData["message"] = "Item Deleted!";
            return RedirectToAction("ItemManagement");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<SelectListItem> getCustomer()
        {
            var custList = new List<SelectListItem>();
            foreach (var cust in db.Customer_tbl)
            {
                custList.Add(new SelectListItem() { Text = cust.Cust_Name, Value = cust.Cust_Id.ToString() });
            }
            return custList;
        }

    }
}
