using Microsoft.AspNetCore.Identity;

namespace HealthCare.Data.Models
{
    public class User : IdentityUser
    {
        public bool IsDeleted { get; set; }
    }
}
