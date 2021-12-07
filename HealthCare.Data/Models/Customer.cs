using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public class Customer : BaseEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string AccountManagerCode { get; set; }
        public string AccountManagerName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
    }
}
