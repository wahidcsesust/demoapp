using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class Loan : BaseEntity
    {
        public int LoanNo { get; set; }
        public long MemberId { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N6}", ApplyFormatInEditMode = true)]
        //[RegularExpression(@"\d{1,10}(\.\d{1,6})", ErrorMessage = "Invalid Longitude")]
        public Decimal InterestRate { get; set; }
        public int Duration { get; set; }
        public decimal Amount { get; set; }
        public decimal Outstanding { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal BaseInstallmentAmount { get; set; }
        public decimal InterestInstallmentAmount { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DateOfCollection { get; set; }
    }
}
