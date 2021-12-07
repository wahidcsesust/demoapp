using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class Department : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
