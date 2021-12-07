using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public class Bed : BaseEntity
    {
        [Required]
        public string BedNo { get; set; }
        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
