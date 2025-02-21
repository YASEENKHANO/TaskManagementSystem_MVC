using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMS_MVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        [Required]
        public string Password { get; set; } // Simple storage (Consider hashing)

        public string Role { get; set; } // Admin, Manager, Employee


        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; } // Simple Hashing Required
        //public string Role { get; set; } // Admin, Manager, Employee
    }
}