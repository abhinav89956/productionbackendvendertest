using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using VenderTest.Models;
using YourProject.Models;

namespace VenderTest.CommonService
{
    public class CommanEmail
    {
        private readonly IConfiguration _configuration;

        public CommanEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

     
        public async Task<bool> SendWelcomeEmailAsync(Vendor vendor, string link)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:Host"];
                var smtpPort = int.Parse(_configuration["EmailSettings:Port"]);
                var smtpUser = _configuration["EmailSettings:Username"];
                var smtpPass = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];

                // Generate email body from template
                string htmlBody = EmailTemplate.GetWelcomeEmailHtml(vendor, link);

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Welcome to Vendor Portal",
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                mail.To.Add(vendor.Email);

                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(mail);
                }

                Console.WriteLine($"Welcome email sent to {vendor.Email}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {vendor.Email}: {ex.Message}");
                return false;
            }
        }
    }
}