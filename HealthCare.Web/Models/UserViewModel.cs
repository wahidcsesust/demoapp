using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthCare.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<SelectListItem> Roles { get; set; }
        [Display(Name = "Role")]
        public string RoleId { get; set; }
    }
}
