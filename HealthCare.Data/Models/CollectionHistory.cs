using System;

namespace HealthCare.Data.Models
{
    public class CollectionHistory : BaseEntity
    {
        public DateTime LastCollectionDate { get; set; }
        public string DateOfCollection { get; set; }
        public long MemberId { get; set; }
    }
}
