using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class Spending
    {
        [Required]
        public int SpendingID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "First Date of Week")]
        public DateTime FirstDateOfWeek { get; set; }

        [Required]

        [Display(Name = "MonSpending")]
        public decimal? MonSpending { get; set; }

        [Required]
        [Display(Name = "TueSpending")]
        public decimal? TueSpending { get; set; }

        [Required]
        [Display(Name = "WedSpending")]
        public decimal? WedSpending { get; set; }

        [Required]
        [Display(Name = "ThuSpending")]
        public decimal? ThuSpending { get; set; }

        [Required]
        [Display(Name = "FriSpending")]
        public decimal? FriSpending { get; set; }

        [Required]
        [Display(Name = "SatSpending")]
        public decimal? SatSpending { get; set; }

        [Required]
        [Display(Name = "SunSpending")]
        public decimal? SunSpending { get; set; }

        [Required]
        [Display(Name = "TotalSpending")]
        public decimal? TotalSpending { get; set; }

        [ForeignKey("UserID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
