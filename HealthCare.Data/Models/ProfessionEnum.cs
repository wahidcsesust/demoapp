using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public enum ProfessionEnum
    {
        [Display(Name = "Job")]
        Job = 1,
        [Display(Name = "Business")]
        Business = 2,
        [Display(Name = "Student")]
        Student = 3,
        [Display(Name = "Farmer")]
        Farmer = 4,
        [Display(Name = "Labourer")]
        Labourer = 5,
        [Display(Name = "Housewife")]
        Housewife = 6,
        [Display(Name = "Other")]
        Other = 7,
    }
}
