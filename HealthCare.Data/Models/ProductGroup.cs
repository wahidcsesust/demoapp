namespace HealthCare.Data.Models
{
    public class ProductGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long CategoryId { get; set; }
    }
}
