using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Purchase
{
    public class PurchaseOrderViewModel
    {
        public long Id { get; set; }
        
        public string PONumber { get; set; }
        [Required]
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
        public string OrderedBy { get; set; }
        public string ReceivedByDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }

        public string BookedBy { get; set; }
        public string BookedByDate { get; set; }
    }
}
