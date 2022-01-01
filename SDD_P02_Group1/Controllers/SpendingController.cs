using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
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
                Console.WriteLine(today);
            }
            
            if (!SpendingContext.IsSpendingExist(userid, today))
            {
                SpendingContext.AddDefaultSpending(userid, today);
            }

            Spending currentWeek = SpendingContext.GetSpendingByDate(userid, today);
            List<Spending> spendingList = SpendingContext.GetAllSpending(userid);

            SpendingViewModel sv = new SpendingViewModel();
            sv.current = currentWeek;
            sv.past = spendingList;
            return View(sv);
        }



    }
}
