using System;

namespace HealthCare.Data.Models
{
    public class LoanCollection : BaseEntity
    {
        public int LoanNo { get; set; }
        public long LoanId { get; set; }
        public long MemberId { get; set; }
        public decimal CollectedAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal Outstanding { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DateOfCollection { get; set; }
    }
}
