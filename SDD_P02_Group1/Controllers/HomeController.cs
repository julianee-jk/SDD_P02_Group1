using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using SDD_P02_Group1.DAL;
using SDD_P02_Group1.Models;
using SDD_P02_Group1.ViewModels;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.DataValidation;
using System.Drawing;

namespace SDD_P02_Group1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserDAL UserContext = new UserDAL();
        private LiabilityDAL LiabilityContext = new LiabilityDAL();
        private SpendingDAL SpendingContext = new SpendingDAL();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Console.WriteLine("Email: " + Request.Cookies["Email"] + " Password: " + Request.Cookies["Password"]);
            List<User> userList = UserContext.GetAllUsers(); // Check judge list
            if ((Request.Cookies["Email"] != "" && Request.Cookies["Email"] != null) && HttpContext.Session.GetString("Role") == null)
            {
                foreach (User user in userList)
                {
                    if (Request.Cookies["Email"] == user.EmailAddr.ToLower() && Request.Cookies["Password"] == user.Password)
                    {
                        // Store Email Address in session with the key “LoginID”
                        HttpContext.Session.SetString("LoginID", Request.Cookies["Email"]);
                        // Store user role “User” as a string in session with the key “Role”
                        HttpContext.Session.SetString("Role", "User");
                        // Store UserId as a int in session with the key “UserID”
                        HttpContext.Session.SetInt32("UserID", user.UserId);
                    }
                }
            }

            if (HttpContext.Session.GetString("Role") == "User")
            {
                int userid = HttpContext.Session.GetInt32("UserID").Value;
                HttpContext.Session.SetString("Username", UserContext.GetDetails(userid).Username);
                ViewData["userEmail"] = UserContext.GetDetails(userid).EmailAddr;

                List<Liability> liabilityList = LiabilityContext.GetAllLiability(userid);

                string message = "";
                DateTime today = DateTime.Now;
                foreach (Liability l in liabilityList)
                {
                    if (l.DueDate != null && ((Convert.ToDateTime(l.DueDate) - today).TotalDays <= 7) && (Convert.ToDateTime(l.DueDate) - today).TotalDays >= 0)
                    {
                        message = message + l.LiabilityName + " (Due Soon)\n";
                    }
                    else if (l.DueDate != null && ((Convert.ToDateTime(l.DueDate) - today).TotalDays < 0))
                    {
                        message = message + l.LiabilityName + " (Overdue)\n";
                    }
                }

                if (message != "" && HttpContext.Session.GetString("LiabilityEmail") != "Sent")
                {
                    message += "\n\n For more information, please log into your Moolah account.";
                    SendEmail("Liabilities Notice", message, UserContext.GetDetails(userid).EmailAddr);
                    HttpContext.Session.SetString("LiabilityEmail", "Sent");
                }

                DateTime currentMonday = DateTime.Today;
                while (currentMonday.DayOfWeek.ToString() != "Monday")
                {
                    currentMonday = currentMonday.AddDays(-1);
                    Console.WriteLine(currentMonday);
                }

                if (!SpendingContext.IsSpendingExist(userid, currentMonday))
                {
                    SpendingContext.AddDefaultSpending(userid, currentMonday);
                }

                Spending currentWeek = SpendingContext.GetSpendingByDate(userid, currentMonday);

                OverviewViewModel sv = new OverviewViewModel();
                sv.lb = liabilityList;
                sv.sp = currentWeek;
                
                return View(sv);
            }      
            return View();
        }

        public ActionResult Login()
        {
            return View();
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
                //Add user record to database
                user.UserId = UserContext.Add(user);
                //Redirect user to Home/Login view
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Input validation fails, return to the Create view to display error message
                return View(user);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult ResetPassword(IFormCollection formData)
        {
            string email = formData["passwordResetEmail"].ToString();
            bool emailExists = UserContext.IsEmailExist2(email);

            if (emailExists == false)
            {
                TempData["NoEmailFoundError"] = "Email does not exist!";
                return View("Login");
            }
            else
            {
                TempData["NoEmailFoundError"] = "";
                Random random = new Random();
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
                string password = "";
                for (int a = 0; a < 15; a++)
                {
                    password = password + Convert.ToString(characters[random.Next(characters.Length)]);
                }


                UserContext.ResetPassword(email, password);

                string messageBody = @"Dear user," + "\n" +
                                          "You are currently attempting a password reset." + "\n" +
                                          "Your password has now been changed to the one below. \n\n" +
                                          password + "\n\n"
                                      + "Please use this password to log in. Note that you can change it later after logging in";
                SendEmail("Reset password", messageBody, email);
                return RedirectToAction("Index");
            }
        }

        public static void SendEmail(string messageBody, string messageContent, string email)
        {
            MailAddress from = new MailAddress("moolahnoreply@gmail.com");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = messageBody;
            message.Body = messageContent;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("moolahnoreply@gmail.com", "moolahpass1234");
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in sending email(): {0}",
                    ex.ToString());
            }
        }

        public ActionResult CreateExcel(string uid, IFormCollection formData)
        {
            int userid = HttpContext.Session.GetInt32("UserID").Value;
            string filepath = "Weekly Spendings - " + userid.ToString() + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                //Add Worksheets in Excel file
                excel.Workbook.Worksheets.Add("Weekly Spendings");
               

                //Create Excel file in Uploads folder of your project
                FileInfo excelFile = new FileInfo(filepath);

                //Add header row columns name in string list array
                var headerRow = new List<string[]>()
                  {
                    new string[] { "First Date Of Week", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Total Spending", "User ID" }
                  };

               
                // Get the header range
                string Range = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                // get the workSheet in which you want to create header
                var worksheet = excel.Workbook.Worksheets["Weekly Spendings"];

                // Populate & style header row data
                worksheet.Cells[Range].Style.Font.Bold = true;
                worksheet.Cells[Range].LoadFromArrays(headerRow);

                //set validation for input boxes
                var decimalValidation = worksheet.DataValidations.AddDecimalValidation("B2:I2");
                decimalValidation.ShowErrorMessage = true;
                decimalValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                decimalValidation.ErrorTitle = "The value you entered is not valid";
                decimalValidation.Error = "This cell must be a valid positive number.";
                decimalValidation.Operator = ExcelDataValidationOperator.greaterThanOrEqual;
                decimalValidation.Formula.Value = 0D;

                //lock all cells exept inputs
                worksheet.Protection.IsProtected = true;
                worksheet.Cells["B2:H2"].Style.Locked = false;
                worksheet.Cells["B2:H2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["B2:H2"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                //add formula to total spent column
                worksheet.Cells["I2"].Formula = "=SUM(B2:H2)";

                //auto size all columns
                worksheet.Cells["A1:K20"].AutoFitColumns();

                //Add data into list
                DateTime currentMonday = DateTime.Today;
                while (currentMonday.DayOfWeek.ToString() != "Monday")
                {
                    currentMonday = currentMonday.AddDays(-1);
                }
                Spending currweek = SpendingContext.GetSpendingByDate(userid, currentMonday);

                //2 is rowNumber 1 is column number
                worksheet.Cells[2, 1].Value = currweek.FirstDateOfWeek.ToString();
                worksheet.Cells[2, 2].Value = ConvertDecimal(currweek.MonSpending.ToString());
                worksheet.Cells[2, 3].Value = ConvertDecimal(currweek.TueSpending.ToString());
                worksheet.Cells[2, 4].Value = ConvertDecimal(currweek.WedSpending.ToString());
                worksheet.Cells[2, 5].Value = ConvertDecimal(currweek.ThuSpending.ToString());
                worksheet.Cells[2, 6].Value = ConvertDecimal(currweek.FriSpending.ToString());
                worksheet.Cells[2, 7].Value = ConvertDecimal(currweek.SatSpending.ToString());
                worksheet.Cells[2, 8].Value = ConvertDecimal(currweek.SunSpending.ToString());
                worksheet.Cells[2, 10].Value = userid;

                //Save Excel file
                excel.SaveAs(excelFile);
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            string fileName = "Weekly Spendings - " + userid.ToString() + ".xlsx";

            //return RedirectToAction("Index");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public decimal ConvertDecimal(string value)
        {
            if(value == null || value == "")
            {
                return 0;
            }
            else { return Convert.ToDecimal(value); }
        }

        public ActionResult AccountLogin(IFormCollection formData, Checkbox check)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string emailAddress = formData["txtEmail"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();
            bool remember = check.remember;

            List<User> userList = UserContext.GetAllUsers(); // Check judge list

            foreach (User user in userList)
            {
                if (emailAddress == user.EmailAddr.ToLower() && password == user.Password)
                {
                    // Store Email Address in session with the key “LoginID”
                    HttpContext.Session.SetString("LoginID", emailAddress);
                    // Store user role “User” as a string in session with the key “Role”
                    HttpContext.Session.SetString("Role", "User");
                    // Store UserId as a int in session with the key “UserID”
                    HttpContext.Session.SetInt32("UserID", user.UserId);

                    // Store login info to cookie if remember me is checked
                    if (remember)
                    {
                      
                        Response.Cookies.Append("Email", emailAddress);
                        Response.Cookies.Append("Password", password);
                    }
                    else
                    {
                        Response.Cookies.Delete("Email");
                        Response.Cookies.Delete("Password");
                    }

                    // Redirect user to the "Index" view through an action
                    return RedirectToAction("Index");
                }
            }

            // Store an error message in TempData for display at the index view
            TempData["LoginErrorMessage"] = "Invalid Login Credentials!";

            // Redirect user back to the index view through an action
            return RedirectToAction("Login");
        }

        public ActionResult SignOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            Response.Cookies.Delete("Email");
            Response.Cookies.Delete("Password");

            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ImportExcel(IFormFile file)
        {
            var weeklySpendingList = new List<WeeklySpending>();
            using (var stream = new MemoryStream())
            {
                if (file != null)
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowcount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowcount; row++)
                        {
                            weeklySpendingList.Add(new WeeklySpending
                            {
                                UserID = Convert.ToInt32(worksheet.Cells[2, 10].Value.ToString().Trim()),
                                FirstDateOfWeek = Convert.ToDateTime(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                MonSpending = Convert.ToDecimal(worksheet.Cells[row, 2].Value.ToString().Trim()),
                                TueSpending = Convert.ToDecimal(worksheet.Cells[row, 3].Value.ToString().Trim()),
                                WedSpending = Convert.ToDecimal(worksheet.Cells[row, 4].Value.ToString().Trim()),
                                ThuSpending = Convert.ToDecimal(worksheet.Cells[row, 5].Value.ToString().Trim()),
                                FriSpending = Convert.ToDecimal(worksheet.Cells[row, 6].Value.ToString().Trim()),
                                SatSpending = Convert.ToDecimal(worksheet.Cells[row, 7].Value.ToString().Trim()),
                                SunSpending = Convert.ToDecimal(worksheet.Cells[row, 8].Value.ToString().Trim()),
                                TotalSpending = Convert.ToDecimal(worksheet.Cells[row, 9].Value.ToString().Trim()),
                            });
                        }
                    }

                    SpendingContext.EditSpending(weeklySpendingList.First());
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
