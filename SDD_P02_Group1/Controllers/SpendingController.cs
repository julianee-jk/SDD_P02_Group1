using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using SDD_P02_Group1.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SDD_P02_Group1.Controllers
{
    public class SpendingController : Controller
    {
        private UserDAL userContext = new UserDAL();
        private SpendingDAL SpendingContext = new SpendingDAL();

        // GET: SpendingController
        public ActionResult Index()
        {
            int userid = HttpContext.Session.GetInt32("UserID").Value;
            DateTime today = DateTime.Today;
            while (today.DayOfWeek.ToString() != "Monday")
            {
                today = today.AddDays(-1);
            }
            
            if (!SpendingContext.IsSpendingExist(userid, today))
            {
                SpendingContext.AddDefaultSpending(userid, today);
            }

            DateTime previousMonday = today.AddDays(-7);

            if (!SpendingContext.IsSpendingExist(userid, previousMonday))
            {
                SpendingContext.AddDefaultSpending(userid, previousMonday);
            }

            Spending currentWeek = SpendingContext.GetSpendingByDate(userid, today);
            Spending previousWeek = SpendingContext.GetSpendingByDate(userid, previousMonday);
            List<Spending> spendingList = SpendingContext.GetAllSpending(userid);

            WeeklySpendingDifference wsd = new WeeklySpendingDifference();
            wsd.MonSpendingDifference = (currentWeek.MonSpending - previousWeek.MonSpending);
            wsd.TueSpendingDifference = (currentWeek.TueSpending - previousWeek.TueSpending);
            wsd.WedSpendingDifference = (currentWeek.WedSpending - previousWeek.WedSpending);
            wsd.ThuSpendingDifference = (currentWeek.ThuSpending - previousWeek.ThuSpending);
            wsd.FriSpendingDifference = (currentWeek.FriSpending - previousWeek.FriSpending);
            wsd.SatSpendingDifference = (currentWeek.SatSpending - previousWeek.SatSpending);
            wsd.SunSpendingDifference = (currentWeek.SunSpending - previousWeek.SunSpending);
            wsd.TotalSpendingDifference = (currentWeek.TotalSpending - previousWeek.TotalSpending);
            wsd.TotalSpendingDifferencePercentage = ((currentWeek.TotalSpending - previousWeek.TotalSpending) / previousWeek.TotalSpending) * 100;

            List<SpendingRecord> recordList = SpendingContext.GetAllSpendingRecord(userid);



            SpendingViewModel sv = new SpendingViewModel();
            sv.current = currentWeek;
            sv.past = spendingList;
            sv.weekdiff = wsd;
            sv.spendrecord = recordList;

            return View(sv);
        }

        // GET: SpendingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SpendingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SpendingRecord record)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userid = HttpContext.Session.GetInt32("UserID").Value;
                    //Add user record to database
                    SpendingContext.AddSpending(record, userid);
                    SpendingContext.UpdateWeeklySpending(userid, record);
                    //Redirect user to Home/Login view
                    return RedirectToAction("Index", "Spending");
                }
                else
                {
                    //Input validation fails, return to the Create view to display error message
                    return RedirectToAction("Create", "Spending");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
