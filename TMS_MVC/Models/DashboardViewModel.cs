using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS_MVC.Models
{
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
        public List<EmployeeTaskCount> TasksPerEmployee { get; set; }
    }

    public class EmployeeTaskCount
    {
        public string EmployeeName { get; set; }
        public int TaskCount { get; set; }
    }
}