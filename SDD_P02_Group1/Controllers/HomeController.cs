using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using Microsoft.AspNetCore.Http;

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
            if (HttpContext.Session.GetString("Role") == "User")
            {
                int userid = HttpContext.Session.GetInt32("UserID").Value;
                HttpContext.Session.SetString("Username", UserContext.GetDetails(userid).Username);
                ViewData["userEmail"] = UserContext.GetDetails(userid).EmailAddr;
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid) 
            {
                //Add user record to database
                user.UserId = UserContext.Add(user);
                //Redirect user to Home/Login view
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Input validation fails, return to the Create view to display error message
                return View(user);
            }
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

        [HttpPost]
        public ActionResult ResetPassword(IFormCollection formData)
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
            string password = "";
            for (int a = 0; a < 15; a++)
            {
                password = password + Convert.ToString(characters[random.Next(characters.Length)]);
            }

            string email = formData["passwordResetEmail"].ToString();
            UserContext.ResetPassword(email, password);

            string messageBody = @"Dear user," + "\n" +
                                      "You are currently attempting a password reset." + "\n" +
                                      "Your password has now been changed to the one below. \n\n" +
                                      password + "\n\n"
                                  + "Please use this password to log in. Note that you can change it later after logging in";
            SendEmail("Reset password", messageBody, email);

            return RedirectToAction("Index");
        }

        public static void SendEmail(string messageBody, string messageContent, string email)
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
                Console.WriteLine("Exception caught in sending email(): {0}",
                    ex.ToString());
            }
        }

        public ActionResult AccountLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string emailAddress = formData["txtEmail"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();

            List<User> userList = UserContext.GetAllUsers(); // Check judge list

            foreach (User user in userList)
            {
                if (emailAddress == user.EmailAddr.ToLower() && password == user.Password)
                {
                    // Store Email Address in session with the key “LoginID”
                    HttpContext.Session.SetString("LoginID", emailAddress);

                    // Store user role “User” as a string in session with the key “Role”
                    HttpContext.Session.SetString("Role", "User");

                    // Store CompetitorId as a int in session with the key “UserID”
                    HttpContext.Session.SetInt32("UserID", user.UserId);

                    // Redirect user to the "Index" view through an action
                    return RedirectToAction("Index");
                }
            }

            //if (emailAddress == "admin1@lcu.edu.sg" & password == "p@55Admin")
            //{
            //    // Store Email Address in session with the key “LoginID”
            //    HttpContext.Session.SetString("LoginID", emailAddress);

            //    // Store user role “Competitor” as a string in session with the key “Role”
            //    HttpContext.Session.SetString("Role", "Admin");

            //    // Redirect user to the "Index" view through an action
            //    return RedirectToAction("Index");
            //}

            // Store an error message in TempData for display at the index view
            TempData["LoginErrorMessage"] = "Invalid Login Credentials!";

            // Redirect user back to the index view through an action
            return RedirectToAction("Login");
        }

        public ActionResult SignOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();

            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }
    }
}
