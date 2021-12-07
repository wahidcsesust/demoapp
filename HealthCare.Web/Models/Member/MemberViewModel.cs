using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HealthCare.Data.Models;

namespace HealthCare.Web.Models.Member
{
    public class MemberViewModel : HealthCare.Data.Models.Member
    {
        [Display(Name = "Reg No")]
        public string RegNoView
        {
            get { return RegNo.ToString().PadLeft(5, '0'); }
            set { value = RegNoView; }
        }
        public string ModalType { get; set; }

        [DisplayName("Blood Group")]
        [EnumDataType(typeof(BloodGroupEnum))]
        public BloodGroupEnum BloodGroupEnum { get; set; }

        [DisplayName("Area Location")]
        [EnumDataType(typeof(AreaLocationEnum))]
        public AreaLocationEnum AreaLocationEnum { get; set; }


        [DisplayName("Member Category")]
        [EnumDataType(typeof(MemberCategoryEnum))]
        public MemberCategoryEnum MemberCategoryEnum { get; set; }
        

        [DisplayName("Designation")]
        [EnumDataType(typeof(MemberDesignationEnum))]
        public MemberDesignationEnum MemberDesignationEnum { get; set; }

        [EnumDataType(typeof(ProfessionEnum))]
        public ProfessionEnum ProfessionEnum { get; set; }

        [EnumDataType(typeof(ReligionEnum))]
        public ReligionEnum ReligionEnum { get; set; }
        public string DateOfBirthStr { get; set; }
    }
}
