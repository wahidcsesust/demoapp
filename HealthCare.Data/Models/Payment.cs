namespace HealthCare.Data.Models
{
    public class Payment : BaseEntity
    {
        public int InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public string PaymentDate { get; set; }
        public long DiagnosisId { get; set; }
        public string Remarks { get; set; }
    }
}
