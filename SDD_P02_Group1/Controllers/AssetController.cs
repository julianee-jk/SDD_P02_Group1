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
    public class AssetController : Controller
    {
        private AssetsDAL assetsContext = new AssetsDAL();
        


        // GET: AssetController
        public ActionResult Index()
        {
            int userid = HttpContext.Session.GetInt32("UserID").Value;
            List<Asset> assetsList = assetsContext.GetAllAsset(userid);
            return View(assetsList);
        }

        // GET: AssetController/Details/5
        public ActionResult Details(int id)
        {
            Asset asset = assetsContext.GetAssetDetails(id);

            if (asset.PredictedValue == null)
            {
                ViewData["pvalue"] = "Not Available";
            }
            return View(asset);
        }

        // GET: AssetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssetController/Create
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

        // GET: AssetController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssetController/Edit/5
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

        // GET: AssetController/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: AssetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            // Delete the staff record from database
            Console.WriteLine("fuckfuckfuckfuckfuckfuckfuckfuck" + id);
            assetsContext.DeleteAsset(id);
            return RedirectToAction("Index");
        }
    }
}
