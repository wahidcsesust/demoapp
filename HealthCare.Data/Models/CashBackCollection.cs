using System;

namespace HealthCare.Data.Models
{
    public class CashBackCollection : BaseEntity
    {
        public long MemberId { get; set; }
        public decimal Amount { get; set; }
        public decimal? CashBackIntAmount { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DateOfCollection { get; set; }
        public string Particulars { get; set; }
    }
}
