using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.StaffViewModel
{
    
    public class StaffViewModel
    {
        public long Id { get; set; }

        [Required]
        public long StaffId { get; set; }

        [Required]
        public string StaffName { get; set; }
        public string DateOfBirth { get; set; }
        [Required]
        public string JoinDate { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public long LastEducation { get; set; }
        public long DepartmentId { get; set; }
        public string Address { get; set; }
        public string NationalID { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public long BankId { get; set; }
        public string DepartmentName { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public string DesignationName { get; set; }
        public List<SelectListItem> Designations { get; set; }
        public string LastEducationName { get; set; }
        public List<SelectListItem> LastEducations { get; set; }
        public string BankName { get; set; }
        public List<SelectListItem> Banks { get; set; }

    }

    public class StaffPictureViewModel
    {
        public long Id { get; set; }
        public long StaffId { get; set; }
        public string StaffPicturePath { get; set; }
        public Byte StaffImage { get; set; }
    }

    public class EducationListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class BankListViewModel
    {
        public long Id { get; set; }
        public string BankName { get; set; }
    }
}
