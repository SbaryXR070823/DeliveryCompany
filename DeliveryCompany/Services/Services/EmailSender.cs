using DeliveryCompany.Services.IServices;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            Log.Information("Sending new e-mail from email, Message:{0}...", message);
            var mail = "ionutalexandrusbaroi@gmail.com";
            var password = "";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            return client.SendMailAsync(new MailMessage(from: mail
            , to: mail,
            subject,
            message));
        }
    }
}

