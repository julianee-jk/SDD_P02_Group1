using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class WeeklySpending
    {

        public DateTime firstDateOfWeek { get; set; }

        public decimal mondaySpending { get; set; }

        public decimal tuesdaySpending { get; set; }

        public decimal wednesdaySpending { get; set; }

        public decimal thursdaySpending { get; set; }

        public decimal fridaySpending { get; set; }

        public decimal saturdaySpending { get; set; }

        public decimal sundaySpending { get; set; }

        public decimal totalSpending { get; set; }

        public int UserID { get; set; }
    }
}
