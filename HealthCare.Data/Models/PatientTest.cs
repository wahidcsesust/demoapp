using System;
using System.Collections.Generic;

namespace HealthCare.Data.Models
{
    public class PatientTest : BaseEntity
    {
        public PatientTest()
        {
            this.PatientTestDetails = new List<PatientTestDetail>();
        }
        public long PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public long DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public long AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? LessAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        //public string DateOfDelivery { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<PatientTestDetail> PatientTestDetails { get; set; }
    }
}
