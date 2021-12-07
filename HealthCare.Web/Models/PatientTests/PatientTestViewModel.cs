using HealthCare.Data.Models;
using HealthCare.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace HealthCare.Web.Models.PatientTests
{
    public class PatientTestViewModel : PatientTest
    {
        public string RegNo { get; set; }
        public string AppointmentNo { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string DeliveryDateString { get; set; }
        public long MedicalTestId { get; set; }
        public IEnumerable<SelectListItem> Appointments { get; set; }
        public IEnumerable<SelectListItem> Patients { get; set; }
        public IEnumerable<SelectListItem> Doctors { get; set; }
        public long NoOfMedicalTests { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal DueAmountHide {
            get
            {
                return (DueAmount = DueAmount ?? 0).Value;
            }
        }
        public string DeliveryDateDisplay
        {
            get
            {
                return (DeliveryDate == null) ? "" : ((DateTime)DeliveryDate).ToString(Constants.DateFormat);
            }
        }
    }
}
