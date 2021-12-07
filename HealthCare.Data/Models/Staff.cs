using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
   public class Staff:BaseEntity
    {
        public long StaffId { get; set; }
        public string StaffName { get; set; }
        public string DateOfBirth { get; set; }
        public string JoinDate { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public long LastEducation { get; set; }
        public long DepartmentId { get; set; }
        public string Address { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public long BankId { get; set; }
        public string NationalId { get; set; }
    }

    public class StaffPicture : BaseEntity
    {
        public long StaffId { get; set; }
        public string StaffPicturePath { get; set; }
        public Byte StaffImage { get; set; }
    }

    public class EducationList : BaseEntity
    {       
        public string Name { get; set; }        
    }

    public class BankList : BaseEntity
    {
        public string BankName { get; set; }
    }


}
