using System.Net;
using System.Net.Mail;
using LibrarySystem.Interface;

namespace LibrarySystem.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "";
            var pass = "";
            var client = new SmtpClient("smtp.gmail.com",587)
            {
                Credentials = new NetworkCredential(mail, pass),
                EnableSsl = true
            };

            return client.SendMailAsync(
                new MailMessage(mail, email, subject, message)
            );
        }
    }
}
