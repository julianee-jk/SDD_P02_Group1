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
            Asset asset = assetContext.GetAssetDetails(id);
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
                    return RedirectToAction("Index", "Asset");
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
            Asset asset = assetContext.GetAssetDetails(id);
            TempData["assetID"] = id;
            return View(asset);
        }

        // POST: AssetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asset asset)
        {
            int userid = HttpContext.Session.GetInt32("UserID").Value;


            if (ModelState.IsValid)
            {
                //Update staff record to database
                //Console.WriteLine("lolol" + TempData["assetID"]);
                //Console.WriteLine("lolol" + asset.CurrentValue);

                Asset a1 = assetContext.GetAssetDetails(Convert.ToInt32(TempData["assetID"]));
                Console.WriteLine(a1.CurrentValue);

                assetContext.EditAsset(asset, Convert.ToInt32(TempData["assetID"]));

                Asset a2 = assetContext.GetAssetDetails(Convert.ToInt32(TempData["assetID"]));       
                Console.WriteLine(a2.CurrentValue);

                assetContext.AddChange(userid, a1, a2);


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

        public ActionResult History()
        {
            if (HttpContext.Session.GetString("Role") != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            int userid = HttpContext.Session.GetInt32("UserID").Value;

            //005
            //List<AssetHistory> staffList = assetContext.GetChanges(userid);

            List<AssetHistory> staffList = assetContext.GetChanges(userid);
            //002
            //return null;
            return View(staffList);
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
