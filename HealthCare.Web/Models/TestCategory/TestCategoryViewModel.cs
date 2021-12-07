using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.TestCategory
{
    public class TestCategoryViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public long NoOfMedicalTests { get; set; }
    }
}
