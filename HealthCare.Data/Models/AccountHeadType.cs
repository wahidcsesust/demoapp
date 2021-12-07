namespace HealthCare.Data.Models
{
    public class AccountHeadType : BaseEntity
    {
        public string Name { get; set; }
        public string AcountType { get; set; }
        public int? TypePrefix { get; set; }
        public string AccountNo { get; set; }
    }
}
