using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models.CompensationViewModel
{
    public class SalaryBreakupViewModel
    {
        public int BreakupId { get; set; }
        public string BreakupName { get; set; }
    }
    public class StaffSalaryStructureDetailsViewModel
    {
        public long StaffId { get; set; }
        public string StructureDate { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }
    }

    public class OtherSalaryBreakupSetupViewModel
    {
        public int BreakupId { get; set; }
        public string BreakupName { get; set; }
        public bool IsEarning { get; set; }
    }

    public class StaffWiseOtherSalaryDetailsViewModel
    {
        public long StaffId { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }
        public bool IsActivated { get; set; }
    }

    public class MonthlyStructureSalaryDetailsViewModel
    {
        public long StaffId { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }

    }

    public class MonthlyOtherSalaryDetailsViewModel
    {
        public long StaffId { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }
        public bool IsEarning { get; set; }

    }
}
