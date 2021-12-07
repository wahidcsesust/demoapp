using System.Collections.Generic;

namespace HealthCare.Data.Models
{
    public class Diagnosis : BaseEntity
    {
        public int DiagNo { get; set; }
        public long PatientId { get; set; }
        public long DoctorId { get; set; }
        public string DiagDate { get; set; }
        public string Description { get; set; }
        public string Advice { get; set; }
        public string Remarks { get; set; }
        //public ICollection<MedicalTest> MedicalTests { get; set; }
    }
}
