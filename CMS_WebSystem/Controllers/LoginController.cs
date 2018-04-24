using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS_WebSystem.Models;

namespace CMS_WebSystem.Controllers
{
    public class LoginController : Controller
    {
        private CMSContext db = new CMSContext();

        // GET: Login
        public ActionResult Index()
        {
            return View(db.User_tbl.ToList());
        }
        public ActionResult InfoPage()
        {
            return View();
        }
        public ActionResult LoginAction(string email, string password)
        {
            if (email == null)
            {
                TempData["message"] = "Pls enter your email";
                return RedirectToAction("Index");
            }
            User_tbl usr_tbl = db.User_tbl.Where(a => a.EmailAddress == email).FirstOrDefault();
            if (usr_tbl == null)
            {
                TempData["message"] = "Invalid email";
                return RedirectToAction("Index");
            }
            else
            {
                if (usr_tbl.Password == password)
                {
                    Session["userId"] = usr_tbl.Id.ToString();
                    Session["userName"] = usr_tbl.Name.ToString();
                    Session["userType"] = usr_tbl.Type.ToString();
                    TempData["message"] = "Login Success";
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["message"] = "Incorrect Password!";
                    return RedirectToAction("Index");
                }
            }

        }

        public ActionResult logOut()
        {
            Session.Abandon();
            TempData["message"] = "You have successfully logged out!";
            return RedirectToAction("Index");
        }

    }
}
