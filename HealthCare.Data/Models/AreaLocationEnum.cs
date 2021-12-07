using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public enum AreaLocationEnum
    {
        [Display(Name = "Purbo Para")]
        East = 1,
        [Display(Name = "Poschim Para")]
        West = 2,
        [Display(Name = "Uttor Para")]
        North = 3,
        [Display(Name = "Dokkhin Para")]
        South = 4,
        [Display(Name = "Mosjid Para")]
        Middle = 5,
        [Display(Name = "Other")]
        Other = 6,
    }
}
