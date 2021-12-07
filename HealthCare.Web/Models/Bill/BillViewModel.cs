using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models.Bill
{
    public class BillViewModel
    {
        public long Id { get; set; }

        [Required]
        public int BillNo { get; set; }

        [Display(Name = "Bill No")]
        public string BillNoView
        {
            get { return "B-" + BillNo.ToString().PadLeft(5, '0'); }
            set { value = BillNoView; }
        }

        [Required]
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public List<SelectListItem> Patients { get; set; }

        [Required]
        public long DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<SelectListItem> Doctors { get; set; }

        public decimal Amount { get; set; }
        public string BillDate { get; set; }
        public string Remarks { get; set; }
    }
}
