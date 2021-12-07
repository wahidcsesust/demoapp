using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HealthCare.Data.Models
{
    public class HelpCollection : BaseEntity
    {
        public int SerialNo { get; set; }
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Gender { get; set; }

        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string NationalIdNo { get; set; }
        public string Religion { get; set; }
        [Required]
        public string Subject { get; set; }

        public long MemberId { get; set; }
        public virtual Member Member { get; set; }

        public DateTime? DateOfHelp { get; set; }
        [NotMapped]
        public string DateOfHelpString { get { return DateOfHelp.HasValue ? DateOfHelp.Value.ToString("dd-MM-yyyy") : string.Empty; } }
    }
}
