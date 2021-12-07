using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Product
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string MeasurementUnit { get; set; }
        public string MeasurementType { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalePrice { get; set; }
        
        [Required]
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; }

        [Required]
        public long ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public List<SelectListItem> ProductGroups { get; set; }

        [Required]
        public long ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
        public List<SelectListItem> ProductBrands { get; set; }
    }
}
