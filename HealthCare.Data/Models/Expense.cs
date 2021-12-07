using System;

namespace HealthCare.Data.Models
{
    public class Expense : BaseEntity
    {
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DateOfCollection { get; set; }
        public string Particulars { get; set; }
    }
}
