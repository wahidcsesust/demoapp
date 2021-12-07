using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCare.Data.Models
{
    public class TransactionDetail : BaseEntity
    {
        public long TransectionId { get; set; }
        public long AccountHeadId { get; set; }
        public bool IsDebit { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
    }
}
