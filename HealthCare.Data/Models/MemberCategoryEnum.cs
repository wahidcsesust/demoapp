using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public enum MemberCategoryEnum
    {
        [Display(Name = "Advisory Council")]
        AdvisoryCouncil = 1,
        [Display(Name = "Executive Council")]
        ExecutiveCouncil = 2,
        [Display(Name = "General Member")]
        GeneralMember = 3,
        [Display(Name = "Other")]
        Other = 4,
    }
}
