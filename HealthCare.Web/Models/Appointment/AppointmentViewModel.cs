using HealthCare.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Appointment
{
    public class AppointmentViewModel : Data.Models.Appointment
    {
        [Display(Name = "Serial No")]
        public string SerialNoView {
            get { return SerialNo.ToString().PadLeft(3, '0'); }
            set { value = SerialNoView; }
        }

        public string PatientName { get; set; }
        public List<SelectListItem> Patients { get; set; }
        
        public string DoctorName { get; set; }
        public List<SelectListItem> Doctors { get; set; }


        [Display(Name = "Department")]
        public string DepartmentName { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public decimal? PaidAmount { get; set; }
        public string AppointmentDateString { get; set; }
        public string AppointmentDateDisplay {
            get 
            {
                return (AppointmentDate == null) ? "" : ((DateTime)AppointmentDate).ToString(Constants.DateFormat);
            }
        }
    }
}
