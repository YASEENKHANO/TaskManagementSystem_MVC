using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TMS_MVC.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // Pending, In Progress, Completed

        public int AssignedTo { get; set; } // Foreign Key
        [ForeignKey("AssignedTo")]
        public virtual User AssignedUser { get; set; } // Navigation Property
    }
}