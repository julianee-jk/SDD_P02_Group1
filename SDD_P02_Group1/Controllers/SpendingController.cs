using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SDD_P02_Group1.Controllers
{
    public class SpendingController : Controller
    {
        private UserCardDAL UserCardContext = new UserCardDAL();
        private UserDAL userContext = new UserDAL();

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
    }
}
