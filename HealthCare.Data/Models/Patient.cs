using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int? Weight { get; set; }
        public int RegNo { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
