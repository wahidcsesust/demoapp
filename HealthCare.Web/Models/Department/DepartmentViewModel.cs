using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models.Department
{
    public class DepartmentViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Department Name")]
        public string Name { get; set; }

        [Display(Name = "Number of Doctors")]
        public long NumberOfDoctors { get; set; }
    }
}
