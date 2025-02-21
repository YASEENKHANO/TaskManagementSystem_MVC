using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace TMS_MVC.Helpers
{

//    [System.Environment]::SetEnvironmentVariable("SMTP_Host", "smtp.gmail.com", "User")
//[System.Environment]::SetEnvironmentVariable("SMTP_Port", "587", "User")
//[System.Environment]::SetEnvironmentVariable("SMTP_User", "your-email@gmail.com", "User")
//[System.Environment]::SetEnvironmentVariable("SMTP_Password", "your-email-password", "User")
//[System.Environment]::SetEnvironmentVariable("SMTP_EnableSSL", "true", "User")




    public class EmailService
    {
        private readonly string smtpHost = System.Configuration.ConfigurationManager.AppSettings["SMTP_Host"];
        private readonly int smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SMTP_Port"]);
        private readonly string smtpUser = System.Configuration.ConfigurationManager.AppSettings["SMTP_User"];
        private readonly string smtpPass = System.Configuration.ConfigurationManager.AppSettings["SMTP_Password"];
        private readonly bool enableSSL = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["SMTP_EnableSSL"]);

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Task Manager", smtpUser));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };
            email.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(smtpUser, smtpPass);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Email sending failed: " + ex.Message);
                }
            }

        }

        }
}