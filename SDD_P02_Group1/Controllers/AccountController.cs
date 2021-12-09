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
using SDD_P02_Group1.ViewModels;
using Microsoft.AspNetCore.Http;
namespace SDD_P02_Group1.Controllers
{
    public class AccountController : Controller
    {
        private UserDAL UserContext = new UserDAL();
        // GET: AccountController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "User")
            {
                int userid = HttpContext.Session.GetInt32("UserID").Value;
                ViewData["userID"] = UserContext.GetDetails(userid).UserId;
                ViewData["userEmail"] = UserContext.GetDetails(userid).EmailAddr;
            }
            return View();
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit()
        {
            int userid = HttpContext.Session.GetInt32("UserID").Value;
            User user = UserContext.GetDetails(userid);

            return View(user);
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                //Update user record to database
                UserContext.EditUser(user, HttpContext.Session.GetInt32("UserID").Value);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(user);
            }
        }
    }
}
