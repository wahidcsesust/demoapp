namespace HealthCare.Data.Models
{
    public class MedicalPaymentDetail : BaseEntity
    {
        public long MedicalPaymentId { get; set; }
        public virtual MedicalPayment MedicalPayment { get; set; }
        public string PaymentType { get; set; }
        public decimal? Amount { get; set; }
    }
}
