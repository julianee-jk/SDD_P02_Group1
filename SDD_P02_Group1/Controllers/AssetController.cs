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
        private AssetsDAL assetContext = new AssetsDAL();
        private UserDAL userContext = new UserDAL();

        // GET: AssetController
        public ActionResult Index()
        {
            int userid = HttpContext.Session.GetInt32("UserID").Value;
            List<Asset> assetsList = assetContext.GetAllAsset(userid);
            return View(assetsList);
        }

        // GET: AssetController/Details/5
        public ActionResult Details(int id)
        {
            Asset asset = assetsContext.GetAssetDetails(id);
            ViewData["editable"] = "false";

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
        public ActionResult Create(Asset asset)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Add user record to database
                    assetContext.AddAsset(asset, HttpContext.Session.GetInt32("UserID").Value);
                    //Redirect user to Home/Login view
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Input validation fails, return to the Create view to display error message
                    return RedirectToAction("Create", "Asset");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: AssetController/Edit/5
        public ActionResult Edit(int id)
        {
            Asset asset = assetsContext.GetAssetDetails(id);
            TempData["assetID"] = id;
            return View(asset);
        }

        // POST: AssetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asset asset)
        {
        
            if (ModelState.IsValid)
            {
                //Update staff record to database
                Console.WriteLine("lolol" + TempData["assetID"]);
                Console.WriteLine("lolol" + asset.CurrentValue);
                assetsContext.EditAsset(asset, Convert.ToInt32(TempData["assetID"]));
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(asset);
            }

/*            if (ViewData["editable"] == "true")
            {
                assetsContext.EditAsset(asset);
                ViewData["editable"] = "false";
                return View();
            }
            else
            {
                ViewData["editable"] = "true";
                return View();
            }*/

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
            assetContext.DeleteAsset(id);
            return RedirectToAction("Index");
        }
    }
}
