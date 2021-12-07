using System;

namespace HealthCare.Data.Models
{
    public class Appointment : BaseEntity
    {
        public string AppointmentNo { get; set; }
        public int SerialNo { get; set; }
        public int SequenceNo { get; set; }
        public long PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public long DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public long DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Problem { get; set; }
        public string Remarks { get; set; }
        public decimal? VisitAmount { get; set; }
        public decimal? DueAmount { get; set; }
    }
}
