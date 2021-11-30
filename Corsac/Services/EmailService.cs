using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Corsac.Services
{
    public static class EmailService
    {
        public static void SendEmail(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            mail.Body = body;
            using (var client = new SmtpClient("localhost", 25))
            {
                client.Send(mail);                
            }
        }
    }
}
