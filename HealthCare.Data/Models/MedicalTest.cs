namespace HealthCare.Data.Models
{
    public class MedicalTest : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public long TestCategoryId { get; set; }
        public virtual TestCategory TestCategory { get; set; }
    }
}
