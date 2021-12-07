using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.ProductBrand
{
    public class ProductBrandViewModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; }

        [Required]
        public long ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public List<SelectListItem> ProductGroups { get; set; }
    }
}
