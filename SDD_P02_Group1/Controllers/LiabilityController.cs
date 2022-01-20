using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using Microsoft.AspNetCore.Http;

namespace SDD_P02_Group1.Controllers
{
    public class LiabilityController : Controller
    {
        private LiabilityDAL LiabilityContext = new LiabilityDAL();
        private UserDAL userContext = new UserDAL();

        // GET: LiabilityController
        public ActionResult Index()
        {
            int userid = HttpContext.Session.GetInt32("UserID").Value;
            List<Liability> liabilityList = LiabilityContext.GetAllLiability(userid);
            return View(liabilityList);
        }

        // GET: LiabilityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LiabilityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LiabilityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Liability liability)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //Add user record to database
                    LiabilityContext.AddLiability(liability, HttpContext.Session.GetInt32("UserID").Value);
                    //Redirect user to Home/Login view
                    return RedirectToAction("Index", "Liability");
                }
                else
                {
                    //Input validation fails, return to the Create view to display error message
                    return RedirectToAction("Create", "Liability");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: LiabilityController/Edit/5
        public ActionResult Edit(int id)
        {
            Liability liability = LiabilityContext.GetLiabilityDetails(id);
            TempData["LiabilityID"] = id;
            return View(liability);
        }

        // POST: LiabilityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Liability liability)
        {
            try
            {
                LiabilityContext.EditLiability(liability, Convert.ToInt32(TempData["assetID"]));
                return RedirectToAction("Index");
            }
            catch
            {
                return View(liability);
            }
        }

        // GET: LiabilityController/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: LiabilityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            LiabilityContext.DeleteLiability(id);
            return RedirectToAction("Index");
        }
    }
}
