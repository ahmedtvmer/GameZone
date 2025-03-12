using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace GameZone.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Use SMTP (e.g., Gmail for testing)
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("ahmedhegazy4me@gmail.com", "chgsfmihdwbqgivy"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("ahmedhegazy4me@gmail.com"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
    
}
