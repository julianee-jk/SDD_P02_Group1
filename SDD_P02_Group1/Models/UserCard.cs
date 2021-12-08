using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class UserCard
    {
        [Required]
        [Display(Name = "Card ID")]
        public int CardID { get; set; }

        [Required]
        [Display(Name = "Card Name")]
        [StringLength(50, ErrorMessage = "Invalid! Name cannot exceed 50 characters")]
        public string CardName { get; set; }

        [Required]
        [Display(Name = "Card Type")]
        [StringLength(50, ErrorMessage = "Invalid! Type cannot exceed 50 characters")]
        public string CardType { get; set; }

        [Required]
        [Display(Name = "Card Description")]
        [StringLength(200, ErrorMessage = "Invalid! Description cannot exceed 200 characters")]
        public string CardDesc { get; set; }

        [Required]
        [ForeignKey("UserID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }

    }
}
