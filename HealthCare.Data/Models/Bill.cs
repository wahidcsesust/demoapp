using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCare.Data.Models
{
    public class Bill : BaseEntity
    {
        public int BillNo { get; set; }
        public decimal Amount { get; set; }
        public long PatientId { get; set; }
        public long DoctorId { get; set; }
        public string BillDate { get; set; }
        public string Remarks { get; set; }
    }
}
