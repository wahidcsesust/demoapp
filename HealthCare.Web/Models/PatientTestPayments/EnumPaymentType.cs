using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.PatientTestPayments
{
    public enum EnumPaymentType
    {
        [Display(Name = "Cash")]
        Cash = 1,
        [Display(Name = "Card")]
        Card
    }
}
