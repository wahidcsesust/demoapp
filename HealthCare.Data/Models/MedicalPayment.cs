using System;
using System.Collections.Generic;

namespace HealthCare.Data.Models
{
    public class MedicalPayment : BaseEntity
    {
        public MedicalPayment()
        {
            this.MedicalPaymentDetails = new List<MedicalPaymentDetail>();
        }
        public long TransactionId { get; set; }
        public string TransactionType { get; set; }
        public long PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string DateOfInvoice { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? LessAmount { get; set; }
        public virtual ICollection<MedicalPaymentDetail> MedicalPaymentDetails { get; set; }
    }
}
