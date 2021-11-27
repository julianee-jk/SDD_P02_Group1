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

        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Invalid! Description cannot exceed 200 characters")]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }


        [Display(Name = "Amount Due")]
        public decimal AmountDue { get; set; }

        [ForeignKey("UserID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
