using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public class Ward : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
