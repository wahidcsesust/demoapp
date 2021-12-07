using System;

namespace HealthCare.Data.Models
{
    public class Collection : BaseEntity
    {
        public long MemberId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DateOfCollection { get; set; }
        public bool IsMainBody { get; set; }
    }
}
