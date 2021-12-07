using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCare.Data.Models
{
    public class FiscalYear : BaseEntity
    {
        public string Name { get; set; }
        public DateTime FiscalYearFrom { get; set; }
        public DateTime FiscalYearTo { get; set; }
        public string FiscalYearPeriod { get; set; }
        public bool IsFiscalYearClosed { get; set; }
    }
}
