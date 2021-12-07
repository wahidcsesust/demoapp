using HealthCare.Data.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Patients
{
    public class PatientsViewModel : Patient
    {
        [Display(Name = "Reg No")]
        public string RegNoView
        {
            get { return RegNo.ToString().PadLeft(6, '0'); }
            set { value = RegNoView; }
        }
        public string ModalType { get; set; }

        [DisplayName("Blood Group")]
        [EnumDataType(typeof(BloodGroupEnum))]
        public BloodGroupEnum BloodGroupEnum { get; set; }
    }
}
