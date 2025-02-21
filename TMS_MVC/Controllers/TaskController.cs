using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS_MVC.Models;

namespace TMS_MVC.Controllers
{
    public class TaskController : Controller
    {
        private bool IsUserLoggedIn()
        {
            return Session["UserId"] != null;
        }
        private bool IsUserInRole(string role)
        {
            return Session["UserRole"] != null && Session["UserRole"].ToString() == role;
        }
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Login"); // Redirect to login if not authenticated

            var tasks = db.Tasks.Include(t => t.AssignedUser).ToList();
            return View(tasks);


            //var tasks = db.Tasks.Include(t => t.AssignedUser).ToList();
            //return View(tasks);

        }

    
        public ActionResult Create()
        {
         
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Login"); 
            
            
            if (!IsUserInRole("Admin") && !IsUserInRole("Manager"))
                return RedirectToAction("Index");


            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Task task)
        {
         
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Login");
            
            
            if (!IsUserInRole("Admin") && !IsUserInRole("Manager"))
                return RedirectToAction("Index");


            db.Tasks.Add(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
          

                if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Login");
            var task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            // Admins & Managers can edit any task, Employees can only edit their own
            if (!IsUserInRole("Admin") && !IsUserInRole("Manager") && (int)Session["UserId"] != task.AssignedTo)
                return RedirectToAction("Index");


            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Name", task.AssignedTo);
            return View(task);
            
        }
        [HttpPost]
        public ActionResult Edit(Task task)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Login");

            // Admins & Managers can edit any task, Employees can only edit their own
            if (!IsUserInRole("Admin") && !IsUserInRole("Manager") && (int)Session["UserId"] != task.AssignedTo)
                return RedirectToAction("Index");



            if (db.Users.Any(u => u.Id == task.AssignedTo)) // Ensure user exists
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Selected user does not exist.");
                ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Name", task.AssignedTo);
                return View(task);
            }
        }

     

        public ActionResult Delete(int id)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Login");
            if (!IsUserInRole("Admin"))
                return RedirectToAction("Index");



            var task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult UpdateStatus(int id, string status)
        {
            var task = db.Tasks.Find(id);
            if (task != null)
            {
                task.Status = status;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}