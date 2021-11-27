using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Controllers
{
    public class LiabilityController : Controller
    {
        // GET: LiabilityController
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LiabilityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LiabilityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LiabilityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiabilityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
