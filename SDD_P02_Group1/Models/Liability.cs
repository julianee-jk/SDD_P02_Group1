using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class Liability
    {
        [Required]
        public int LiabilityID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Invalid! Name cannot exceed 50 characters")]
        [Display(Name = "Liability Name")]
        public string LiabilityName { get; set; }

        [Required]
        [Display(Name = "Type")]
        [StringLength(50, ErrorMessage = "Invalid! Type cannot exceed 50 characters")]
        public string LiabilityType { get; set; }

        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Invalid! Description cannot exceed 200 characters")]
        public string? LiabilityDesc { get; set; }

        [Required]
        [Display(Name = "Cost")]
        public decimal Cost { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        [Required]
        [Display(Name = "Recurring Type")]
        [StringLength(50, ErrorMessage = "Invalid! Recurring type cannot exceed 50 characters")]
        public string RecurringType { get; set; }

        [Display(Name = "Recurring Duration")]
        public int? RecurringDuration { get; set; }

        [ForeignKey("UserID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
