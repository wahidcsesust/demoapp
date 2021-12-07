using System;

namespace HealthCare.Data.Models
{
    public class MiscellaneousCollection : BaseEntity
    {
        public long MiscellaneousTypeId { get; set; }
        public long MemberId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DateOfCollection { get; set; }
        public string Particulars { get; set; }
    }
}
