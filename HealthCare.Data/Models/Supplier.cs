using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class Supplier : BaseEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string SupName { get; set; }
        [Required]
        public string SupAddress { get; set; }
        [Required]
        public string OfficePhone { get; set; }
        public string OfficeMail { get; set; }
        public string WebAddress { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        public string ContactPersonAddress { get; set; }
        [Required]
        public string ContactPersonMobile { get; set; }
        public string ContactPersonMail { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsPaymentBeforeSale { get; set; }
        public string PaymentDay { get; set; }
        public bool IsPaymentAfterSale { get; set; }
        public bool Status { get; set; }
    }
}
