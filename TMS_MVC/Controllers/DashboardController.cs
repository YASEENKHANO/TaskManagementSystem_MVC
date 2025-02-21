using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TMS_MVC.Models;
namespace TMS_MVC.Controllers
{
    public class DashboardController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalTasks = db.Tasks.Count(),
                PendingTasks = db.Tasks.Count(t => t.Status == "Pending"),
                InProgressTasks = db.Tasks.Count(t => t.Status == "In Progress"),
                CompletedTasks = db.Tasks.Count(t => t.Status == "Completed"),
                TasksPerEmployee = db.Tasks
          .GroupBy(t => t.AssignedTo)
          .Select(group => new EmployeeTaskCount
          {
              EmployeeName = db.Users.Where(u => u.Id == group.Key).Select(u => u.Name).FirstOrDefault(),
              TaskCount = group.Count()
          })
          .ToList()
            };

            return View(model);
            //var totalTasks = db.Tasks.Count();
            //var pendingTasks = db.Tasks.Count(t => t.Status == "Pending");
            //var completedTasks = db.Tasks.Count(t => t.Status == "Completed");

            //var tasksPerEmployee = db.Tasks
            //    .GroupBy(t => t.AssignedTo)
            //    .Select(group => new
            //    {
            //        EmployeeId = group.Key,
            //        TaskCount = group.Count(),
            //        EmployeeName = db.Users.Where(u => u.Id == group.Key).Select(u => u.Name).FirstOrDefault()
            //    })
            //    .ToList();

            //ViewBag.TotalTasks = totalTasks;
            //ViewBag.PendingTasks = pendingTasks;
            //ViewBag.CompletedTasks = completedTasks;
            //ViewBag.TasksPerEmployee = tasksPerEmployee;

            //return View();
        }
    }
}