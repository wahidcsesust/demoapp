using HealthCare.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models.Admission
{
    public class AdmissionViewModel : PatientAdmission
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string RoomNo { get; set; }
        public string BedNo { get; set; }
        public string Location { get; set; }
        public string AdmissionDateString { get; set; }
        public string DischargeDateString { get; set; }
        public string OperationDateString { get; set; }
        public IEnumerable<SelectListItem> Patients { get; set; }
        public IEnumerable<SelectListItem> Doctors { get; set; }
        public IEnumerable<SelectListItem> Beds { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal DueAmountHide
        {
            get
            {
                return (DueAmount = DueAmount ?? 0).Value;
            }
        }
    }
}
