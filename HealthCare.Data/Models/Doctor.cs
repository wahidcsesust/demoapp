using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class Doctor : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Reg Number")]
        public string BmdcRegNumber { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string Specilization { get; set; }
        [Display(Name = "Visit Price")]
        public int VisitPrice { get; set; }
        [Phone]
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string DegreeOther { get; set; }

        public long DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
