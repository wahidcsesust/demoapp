using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class Room : BaseEntity
    {
        [Required]
        public string RoomNo { get; set; }

        public string Location { get; set; }
    }
}
