using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
