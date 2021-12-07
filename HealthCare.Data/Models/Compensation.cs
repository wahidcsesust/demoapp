using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{

    public class SalaryBreakup : BaseEntity
    {
        public int BreakupId { get; set; }
        public string BreakupName { get; set; }
    }
    public class StaffSalaryStructureDetails : BaseEntity
    {
        public long StaffId { get; set; }
        public string StructureDate { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }        
    }

    public class OtherSalaryBreakupSetup : BaseEntity
    {
        public int BreakupId { get; set; }
        public string BreakupName { get; set; }
        public bool IsEarning { get; set; }
    }

    public class StaffWiseOtherSalaryDetails : BaseEntity
    {
        public long StaffId { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }
        public bool IsActivated { get; set; }
    }

    public class MonthlyStructureSalaryDetails : BaseEntity
    {
        public long StaffId { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }        
    }

    public class MonthlyOtherSalaryDetails : BaseEntity
    {
        public long StaffId { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public int BreakupId { get; set; }
        public double BreakupAmount { get; set; }
        public bool IsEarning { get; set; }

    }
}
