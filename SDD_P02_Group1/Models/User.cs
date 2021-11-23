using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class User
    {
        [Display(Name = "ID")]
        public int UserId { get; set; }

        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password!")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password must be 8 - 40 characters long.")]
        [MaxLength(40)]
        public string Password { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please enter an email address!")]
        [StringLength(50, ErrorMessage = "Email Address cannot exceed 50 characters")]
        [EmailAddress]
        [ValidateUserEmailExists]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Invalid Email Address!")]
        public string EmailAddr { get; set; }
    }
}
