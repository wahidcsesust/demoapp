using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public enum ReligionEnum
    {
        [Display(Name = "Islam")]
        Islam = 1,
        [Display(Name = "Hinduism")]
        Hinduism = 2,
        [Display(Name = "Buddhism")]
        Buddhism = 3,
        [Display(Name = "Christianity")]
        Christianity = 4,
        [Display(Name = "Other")]
        Other = 5,
    }
}
