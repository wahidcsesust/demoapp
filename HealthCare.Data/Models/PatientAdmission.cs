using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HealthCare.Data.Models
{
    public class PatientAdmission : BaseEntity
    {
        public long PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public long? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public long? RoomId { get; set; }
        public virtual Room Room { get; set; }

        public long? BedId { get; set; }
        public virtual Bed Bed { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string AdmissionTime { get; set; }

        public DateTime? DischargeDate { get; set; }
        public string DischargeTime { get; set; }
        public string CareOf { get; set; }
        public string Remarks { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? LessAmount { get; set; }

        public string OperationRegNo { get; set; }
        public DateTime? OperationDate { get; set; }
        public string OperationTime { get; set; }
        public string OperationName { get; set; }
        public string Indication { get; set; }
        public string Incision { get; set; }
        public string Findings { get; set; }

        public string Sex { get; set; }
        public int? Weight { get; set; }
        public string ApgarScore { get; set; }
        public string Others { get; set; }
        public string BloodTransfusion { get; set; }
        public string Surgeon { get; set; }
        public string Assistant { get; set; }
        public string Anaesthesiologist { get; set; }
        public string AnaesthesiaType { get; set; }
    }
}
