using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS_MVC.Models;

namespace TMS_MVC.Controllers
{
    public class LoginController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Login() => View();


        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Store user details in session
                Session["UserId"] = user.Id;
                Session["UserName"] = user.Name;
                Session["UserRole"] = user.Role;

                return RedirectToAction("Index", "Task");
            }

            ViewBag.Message = "Invalid credentials!";
            return View();
        }
        //[HttpPost]
        //public ActionResult Login(string email, string password)
        //{
        //    var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        //    if (user != null)
        //    {
        //        Session["UserId"] = user.Id;
        //        Session["Role"] = user.Role;
        //        return RedirectToAction("Index", "Task");
        //    }
        //    ViewBag.Message = "Invalid credentials!";
        //    return View();
        //}

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}