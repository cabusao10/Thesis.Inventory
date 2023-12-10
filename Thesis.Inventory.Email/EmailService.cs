using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Email.Config;

namespace Thesis.Inventory.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceConfig _config;
        public EmailService()
        {
            
        }
        public bool SendEmail(string receiver_email, string body)
        {
            var sender = new MailAddress("cabusaorhen14@gmail.com", "Admin");
            var receiver = new MailAddress(receiver_email, "User");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender.Address, "ijlo rarm hfot lvzz"),
            };

            using (var message = new MailMessage(sender, receiver)
            {
                Subject = "Verification",
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
            return true;
        }
    }
}
