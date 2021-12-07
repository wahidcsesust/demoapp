using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Data.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long CategoryId { get; set; }
        public long ProductGroupId { get; set; }
        public long ProductBrandId { get; set; }
        public Category Category { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public ProductBrand ProductBrand { get; set; }
        [NotMapped]
        public string ProductGroupName { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public string ProductBrandName { get; set; }
        public string MeasurementUnit { get; set; }
        public string MeasurementType { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalePrice { get; set; }
    }
}
