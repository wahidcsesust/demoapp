using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HealthCare.Data.Models
{
    public class PharmacyIncome : BaseEntity
    {
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime? CollectionDate { get; set; }
        [NotMapped]
        public string CollectionDateString { get; set; }
        public string Particulars { get; set; }
    }
}
