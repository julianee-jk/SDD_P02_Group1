using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class WeeklySpendingDifference
    {
        [Required]
        [Display(Name = "MonSpendingDifference")]
        public decimal? MonSpendingDifference { get; set; }

        [Required]
        [Display(Name = "TueSpendingDifference")]
        public decimal? TueSpendingDifference { get; set; }

        [Required]
        [Display(Name = "WedSpendingDifference")]
        public decimal? WedSpendingDifference { get; set; }

        [Required]
        [Display(Name = "ThuSpendingDifference")]
        public decimal? ThuSpendingDifference { get; set; }

        [Required]
        [Display(Name = "FriSpendingDifference")]
        public decimal? FriSpendingDifference { get; set; }

        [Required]
        [Display(Name = "SatSpendingDifference")]
        public decimal? SatSpendingDifference { get; set; }

        [Required]
        [Display(Name = "SunSpendingDifference")]
        public decimal? SunSpendingDifference { get; set; }

        [Required]
        [Display(Name = "TotalSpendingDifference")]
        public decimal? TotalSpendingDifference { get; set; }

        [Required]
        [Display(Name = "TotalSpendingDifference(%)")]
        public decimal? TotalSpendingDifferencePercentage { get; set; }
    }
}
