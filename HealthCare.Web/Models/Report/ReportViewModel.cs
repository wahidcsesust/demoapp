using HealthCare.Data.Models;
using HealthCare.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Report
{
    public class ReportViewModel
    {
        [Required]
        public long ObjectId { get; set; }
        public string ObjectName { get; set; }
        public IEnumerable<SelectListItem> Objects { get; set; }

        [EnumDataType(typeof(MemberCategoryEnum))]
        public MemberCategoryEnum MemberCategoryEnum { get; set; }

        [EnumDataType(typeof(BloodGroupEnum))]
        public BloodGroupEnum BloodGroupEnum { get; set; }

        [EnumDataType(typeof(AreaLocationEnum))]
        public AreaLocationEnum AreaLocationEnum { get; set; }


        public string FatherName { get; set; }
        public string ImageName { get; set; }
        public string Category { get; set; }
        public string Designation { get; set; }
        public string BloodGroup { get; set; }
        public string Profession { get; set; }
        public int Age { get; set; }
        public string MobileNumber { get; set; }


        [Required]
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public IEnumerable<SelectListItem> Patients { get; set; }

        public long ExpenseId { get; set; }
        public string Invoice { get; set; }
        public string Particular { get; set; }
        public IEnumerable<SelectListItem> Expenses { get; set; }

        public int SerialNo { get; set; }
        public int RegNo { get; set; }
        public string RegNoView
        {
            get { return RegNo.ToString().PadLeft(5, '0'); }
            set { value = RegNoView; }
        }

        public string TransactionType { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? LessAmount { get; set; }
        public decimal? PaidAmount { get; set; }

        [Required]
        public long MemberId { get; set; }
        public string MemberName { get; set; }
        public IEnumerable<SelectListItem> Members { get; set; }

        [Required]
        public long MiscellaneousTypeId { get; set; }
        public string MiscellaneousTypeName { get; set; }
        public IEnumerable<SelectListItem> MiscellaneousTypes { get; set; }

        public DateTime CollectionDate { get; set; }
        public string CollectionDateString { get { return (CollectionDate == null) ? "" : ((DateTime)CollectionDate).ToString(Constants.DateFormat); ; } }
        public string DateOfCollection { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string DateOfAppointment { get; set; }
        public string AppointmentDateString { get; set; }


        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        [EnumDataType(typeof(ReportType))]
        public ReportType ReportType { get; set; }
        [EnumDataType(typeof(ReportTypeEnum))]
        public ReportTypeEnum ReportTypeEnum { get; set; }
        public string MonthName { get; set; }
        public decimal Amount { get; set; }
        public bool IsMemberInfoDisplay { get; set; }
        public string ReportHeading { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string InvoiceDateString { get { return (InvoiceDate == null) ? "" : ((DateTime)InvoiceDate).ToString(Constants.DateFormat); ; } }
    }
    public enum ReportType
    {
        DoctorAppointment = 1,
        DoctorVisit = 2,
        MedicalTest = 3,
        Admission = 4,
        ExpenseReport = 5,
        PharmacyReport = 6
    }
    public enum ReportTypeEnum
    {
        MemberReport = 1,
    }
}
