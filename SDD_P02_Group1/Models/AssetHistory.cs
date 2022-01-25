using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace SDD_P02_Group1.Models
{
    public class AssetHistory
    {
        //[ForeignKey("UserID")]
        public int UserID { get; set; }
        //public virtual User User { get; set; }

        //[Required]
        public int AssetID { get; set; }

        public DateTime Timestamp { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "Invalid! Type cannot exceed 50 characters")]
        [Display(Name = "Old Asset Type")]
        public string AssetType { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "Invalid! Type cannot exceed 50 characters")]
        [Display(Name = "New Asset Type")]
        public string? AssetTypeNew { get; set; }

        //[StringLength(200, ErrorMessage = "Invalid! Description cannot exceed 50 characters")]
        [Display(Name = "Old Asset Description")]
        public string AssetDesc { get; set; }

        //[StringLength(200, ErrorMessage = "Invalid! Description cannot exceed 50 characters")]
        [Display(Name = "New Asset Description")]
        public string? AssetDescNew { get; set; }

        //[Required]
        [Display(Name = "Old Current Value")]
        public decimal  CurrentValue { get; set; }

        //[Required]
        [Display(Name = "New Current Value")]
        public decimal? CurrentValueNew { get; set; }
    }
}
