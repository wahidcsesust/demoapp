namespace HealthCare.Data.Models
{
    public class AccountHead : BaseEntity
    {
        public long AccountHeadTypeId { get; set; }
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal? ActualCurrentBalance { get; set; }
    }
}
