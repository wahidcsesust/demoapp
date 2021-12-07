using System;

namespace HealthCare.Data.Models
{
    public class PurchaseOrder : BaseEntity
    {
        public string PONumber { get; set; }
        public long SupplierId { get; set; }
        public string OrderedBy { get; set; }
        public string ReceivedByDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string BookedBy { get; set; }
        public string BookedByDate { get; set; }
    }
}
