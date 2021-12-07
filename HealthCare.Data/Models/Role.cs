using Microsoft.AspNetCore.Identity;

namespace HealthCare.Data.Models
{
    public class Role : IdentityRole
    {
        public bool IsDeleted { get; set; }
    }
}
