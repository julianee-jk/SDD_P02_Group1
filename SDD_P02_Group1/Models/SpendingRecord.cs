using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class SpendingRecord
    {
        [Required]
        public int RecordID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Transaction")]
        public DateTime DateOfTransaction { get; set; }

        [Required]
        [Display(Name = "SpendingCategory")]
        public string CategoryOfSpending { get; set; }

        [Required]
        [Display(Name = "AmountSpent")]
        public decimal? AmountSpent { get; set; }

        [ForeignKey("UserID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
