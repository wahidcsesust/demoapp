using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public enum BloodGroupEnum
    {
        [Display(Name = "NONE")]
        NONE = 1,
        [Display(Name = "A +ve")]
        APositive = 2,
        [Display(Name = "B +ve")]
        BPostive = 3,
        [Display(Name = "AB +ve")]
        ABPositive = 4,
        [Display(Name = "O +ve")]
        OPositive = 5,
        [Display(Name = "A -ve")]
        ANegative = 6,
        [Display(Name = "B -ve")]
        BNegative = 7,
        [Display(Name = "AB -ve")]
        ABNegative = 8,
        [Display(Name = "O -ve")]
        ONegative = 9
    }
}
