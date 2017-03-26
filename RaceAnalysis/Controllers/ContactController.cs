using Microsoft.AspNet.Identity;
using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{
    public class ContactController : Controller
    {
        IIdentityMessageService _emailService;
        public ContactController(IIdentityMessageService emailService)
        {
            _emailService = emailService;
        }

        // GET: Contact
        public ActionResult Index()
        {
            return View(new ContactForm());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Submit(ContactForm model)
        {
            bool isMessageSent = true;

            if (ModelState.IsValid)
            {
                string bodyText = "<p>The following is a message from {0} ({1})</p><p>{2}</p>";
                string body = string.Format(bodyText, model.Name, model.Email, model.Message);

                try
                {
                    await _emailService.SendAsync(
                        new IdentityMessage
                        {
                            Subject = "Contact Form Message",
                            Destination = "scott_clemmons@hotmail.com",
                            Body = body
                        }
                     );

                   // await RaceAnalysis.Service.EmailService.SendContactForm(model);
                }
                catch (Exception )
                {
                    isMessageSent = false;
                }
            }
            else
            {
                isMessageSent = false;
            }
            return PartialView("_SubmitMessage", isMessageSent);
        }
    }
}