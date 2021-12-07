using System;

namespace HealthCare.Data.Models
{
    public class Transaction : BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
