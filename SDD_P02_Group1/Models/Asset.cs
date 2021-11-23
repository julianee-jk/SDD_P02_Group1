using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.Models
{
    public class Asset
    {
        [Required]
        public int AssetID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Invalid! Name cannot exceed 50 characters")]
        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }

        [Required]
        [Display(Name = "Initial value")]
        public decimal InitialValue { get; set; }  
        
        [Required]
        [Display(Name = "Current value")]
        public decimal CurrentValue { get; set; }        

        [Display(Name = "Predicted value")]
        public decimal? PredictedValue { get; set; }

        [ForeignKey("UserID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
