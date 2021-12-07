using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Doctor
{
    public class DoctorViewModel : Data.Models.Doctor
    {
        public string DepartmentName { get; set; }
        public List<SelectListItem> Departments { get; set; }
    }
}
