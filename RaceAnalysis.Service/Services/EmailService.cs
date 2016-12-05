using Microsoft.AspNet.Identity;
using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Service
{
        public class EmailService : IIdentityMessageService
        {
            public async Task SendAsync(IdentityMessage message)
            {
                var email =
                   new MailMessage(new MailAddress("noreply@mydomain.com", "(do not reply)"),
                   new MailAddress(message.Destination))
                   {
                       Subject = message.Subject,
                       Body = message.Body,
                       IsBodyHtml = true
                   };

                using (var client = new SmtpClient()) // SmtpClient configuration comes from config file
                {
                    await client.SendMailAsync(email);
                }
            }
        }
    
    
}
