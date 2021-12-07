using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.MedicalTest
{
    public class MedicalTestViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }

        [Display(Name = "Medical Test Name")]
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public long TestCategoryId { get; set; }
        public string TestCategoryName { get; set; }
        public List<SelectListItem> TestCategories { get; set; }
    }
}
