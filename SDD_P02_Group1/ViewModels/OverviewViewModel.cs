using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDD_P02_Group1.Models;

namespace SDD_P02_Group1.ViewModels
{
    public class OverviewViewModel
    {
        public List<Liability> lb { get; set; }

        public Spending sp { get; set; }
    }
}
