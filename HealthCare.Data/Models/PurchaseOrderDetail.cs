namespace HealthCare.Data.Models
{
    public class PurchaseOrderDetail : BaseEntity
    {
        public long POId { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public string Comment { get; set; }
        public bool IsComplete { get; set; }
    }
}
