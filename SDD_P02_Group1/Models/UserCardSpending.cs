using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class UserCardSpending
    {
        [Required]
        [Display(Name = "CardSpending ID")]
        public int CardSpendingID { get; set; }

        public DateTime DateOfTransaction { get; set; }

        public decimal AmountSpent { get; set; }

        [Display(Name = "Total Card Spendings")]
        public decimal TotalCardAmountSpent { get; set; }

        [Required]
        [ForeignKey("CardID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }

    }
}
