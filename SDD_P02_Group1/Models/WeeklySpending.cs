using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class WeeklySpending
    {

        public DateTime FirstDateOfWeek { get; set; }

        public decimal MonSpending { get; set; }

        public decimal TueSpending { get; set; }

        public decimal WedSpending { get; set; }

        public decimal ThuSpending { get; set; }

        public decimal FriSpending { get; set; }

        public decimal SatSpending { get; set; }

        public decimal SunSpending { get; set; }

        public decimal TotalSpending { get; set; }

        public int UserID { get; set; }
    }
}
