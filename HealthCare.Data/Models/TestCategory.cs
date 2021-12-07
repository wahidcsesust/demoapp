using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class TestCategory : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<MedicalTest> MedicalTests {get;set;}
    }
}
