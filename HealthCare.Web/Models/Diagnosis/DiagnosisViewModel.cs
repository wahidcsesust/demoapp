using HealthCare.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Diagnosis
{
    public class DiagnosisViewModel
    {
        public long Id { get; set; }

        [Required]
        public int DiagNo { get; set; }

        [Display(Name = "Diag No")]
        public string DiagNoView
        {
            get { return DiagNo.ToString().PadLeft(3, '0'); }
            set { value = DiagNo.ToString().PadLeft(3, '0'); }
        }

        [Required]
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public List<SelectListItem> Patients { get; set; }

        [Required]
        public long DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<SelectListItem> Doctors { get; set; }
        
        public string DiagDate { get; set; }
        public string Description { get; set; }
        public string Advice { get; set; }
        public string Remarks { get; set; }
        //public ICollection<MedicalTest> MedicalTests { get; set; }
    }
}
