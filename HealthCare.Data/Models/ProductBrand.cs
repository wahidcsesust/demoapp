using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Data.Models
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long CategoryId { get; set; }
        public long ProductGroupId { get; set; }
        public Category Category { get; set; }
        public ProductGroup ProductGroup { get; set; }
    }
}
