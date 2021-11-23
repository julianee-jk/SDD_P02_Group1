using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using Microsoft.AspNetCore.Http;

namespace WEB2021Apr_P02_T1.Controllers
{
    public class UserController : Controller
    {
        private UserDAL userContext = new UserDAL();

        public ActionResult Index()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<User> userList = userContext.GetAllUsers();
            return View(userList);
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
                //Add judge record to database
                user.UserId = userContext.Add(user);
                //Redirect user to Home/Login view
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(user);
            }
        }
    }
}
