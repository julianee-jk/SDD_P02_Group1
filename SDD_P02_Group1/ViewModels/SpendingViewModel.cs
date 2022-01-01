using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace SDD_P02_Group1.Models
{
    public class SpendingViewModel
    {
        public Spending current { get; set; }
        public List<Spending> past { get; set; }
    }
}
