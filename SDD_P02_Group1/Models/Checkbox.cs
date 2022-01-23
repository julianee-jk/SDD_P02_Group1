using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class Checkbox
    {
        [Display(Name = "Remember Me")]
        public bool remember { get; set; }
    }
}
