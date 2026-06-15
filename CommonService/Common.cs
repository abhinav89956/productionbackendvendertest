using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace VenderTest.CommonService
{
    public class Common
    {
        private readonly IConfiguration _configuration;

        public Common(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendOtpEmailAsync(string toEmail, string otp)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:Host"];
                var smtpPort = int.Parse(_configuration["EmailSettings:Port"]);
                var smtpUser = _configuration["EmailSettings:Username"];
                var smtpPass = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];

                string htmlBody = OtpTemplate.GetOtpTemplate(otp);

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Reset Your Password - Vendor Portal",
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                mail.To.Add(toEmail);

                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.UseDefaultCredentials = false;   
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(mail);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
