using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserDAL UserContext = new UserDAL();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

/*        [HttpPost]
        public ActionResult resetPassword(IFormCollection formData)
        {
            // still need put the automatic new password here
            //--------------------------------------------------

            string email = formData["txtEmail"].ToString();
           *//*UserContext.ResetPassword(email, newpassword);*//*

            string messageBody = @"Dear user, \n" +
                                      "You are currently attempting a password reset. \n" + "\n" +
                                      "Please click the link below to verify the authenticity of this action. \n" +
                                      "https://localhost:44363/" + "\n"
                                  + "For the new password, it will be given to you only after verification";

            sendEmail("Reset password", messageBody, email);

            return RedirectToAction("");
        } */

        public static void sendEmail(string messageBody, string messageContent, string email)
        {
            MailAddress from = new MailAddress("moolahnoreply@gmail.com");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = messageBody;
            message.Body = messageContent;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("moolahnoreply@gmail.com", "moolahpass1234");
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage4(): {0}",
                    ex.ToString());
            }
        }
    }
}
