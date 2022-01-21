using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SDD_P02_Group1.ViewModels;

namespace SDD_P02_Group1.Controllers
{
    public class UserCardController : Controller
    {
        private UserCardDAL UserCardContext = new UserCardDAL();
        private UserCardDAL userCardContext = new UserCardDAL();

        // GET: LiabilityController
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = HttpContext.Session.GetInt32("UserID").Value;
            List<UserCard> userCardList = UserCardContext.GetAllUserCard(userid);
            return View(userCardList);
        }

        //GET: LiabilityController/Details/5
        public ActionResult Details(int? cardid, int? userid)
        {

            UserCard usercard = userCardContext.GetUserCardDetails(userid.Value, cardid.Value);
            UserCardSpending userCardSpending = userCardContext.GetUserCardSpendingsDetails(userid.Value, cardid.Value);

            UserCardSpendingViewModel userCardSpendingVM = new UserCardSpendingViewModel();

            userCardSpendingVM.userCard = usercard;
            userCardSpendingVM.userCardSpending = userCardSpending;

            return View(userCardSpendingVM);
        }

        // GET: LiabilityController/Create
        public ActionResult Create()
        {
            ViewData["CardType"] = GetCardTypes();
            return View();
        }

        // POST: LiabilityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCard usercard)
        {
            try
            {
                ViewData["CardType"] = GetCardTypes();
                if (ModelState.IsValid)
                {

                    //Add user record to database
                    UserCardContext.AddUserCard(usercard, HttpContext.Session.GetInt32("UserID").Value);
                    //Redirect user to UserCard/Index view
                    return RedirectToAction("Index", "UserCard");
                }
                else
                {
                    //Input validation fails, return to the Create view to display error message
                    return RedirectToAction("Create", "UserCard");
                }
            }
            catch
            {
                ViewData["CardType"] = GetCardTypes();
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
        public ActionResult Delete(int userid, int cardid)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            UserCard userCard = userCardContext.GetUserCardDetails(userid, cardid);
            if (userCard == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(userCard);
        }

        // POST: LiabilityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserCard userCard)
        {
            userCardContext.Delete(userCard.UserID, userCard.CardID);
            return RedirectToAction("Index");
        }


        // Get all card types list
        private List<SelectListItem> GetCardTypes()
        {
            List<SelectListItem> cardTypes = new List<SelectListItem>();
            cardTypes.Add(new SelectListItem
            {
                Value = "Credit",
                Text = "Credit"
            });
            cardTypes.Add(new SelectListItem
            {
                Value = "Debit",
                Text = "Debit"
            });
            return cardTypes;
        }
    }
}
