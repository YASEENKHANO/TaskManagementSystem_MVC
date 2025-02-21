using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TMS_MVC.Helpers;
using TMS_MVC.Models;

namespace TMS_MVC.Controllers
{
    public class AuthController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // Display the Registration Form
        public ActionResult Register()
        {
            return View();
        }

        // Handle Registration Form Submission
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            if (db.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("", "Email is already in use.");
                return View(user);
            }

            user.Role = "Employee";
            user.IsEmailVerified = false; // Add this column in your User table
            db.Users.Add(user);
            db.SaveChanges();

            // Send verification email
            string verificationLink = Url.Action("VerifyEmail", "Auth", new { email = user.Email }, Request.Url.Scheme);
            string emailBody = $"<p>Please <a href='{verificationLink}'>click here</a> to verify your email.</p>";

            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Email Verification", emailBody);

            ViewBag.Message = "Registration successful! Please check your email for verification.";
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear(); // Destroy user session
            return RedirectToAction("Login");
        }

        public ActionResult VerifyEmail(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.IsEmailVerified = true;
                db.SaveChanges();
                ViewBag.Message = "Your email has been verified! You can now login.";
            }
            else
            {
                ViewBag.Message = "Invalid verification link.";
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Message = "No account found with this email.";
                return View();
            }

            // Generate Password Reset Link
            string resetLink = Url.Action("ResetPassword", "Auth", new { email = user.Email }, Request.Url.Scheme);
            string emailBody = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";

            // Send Email
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Password Reset Request", emailBody);

            ViewBag.Message = "Password reset link has been sent to your email.";
            return View();
        }


        public ActionResult ResetPassword(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.Email = email; // Pass email to the view
            return View();
            //var user = db.Users.FirstOrDefault(u => u.Email == email);
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Email = email;
            //return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string email, string newPassword)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Password = newPassword; // Ideally, hash the password before saving
                db.SaveChanges();
                ViewBag.Message = "Your password has been reset successfully!";
            }
            else
            {
                ViewBag.Message = "Invalid request.";
            }
            return View();
        }


    }
}