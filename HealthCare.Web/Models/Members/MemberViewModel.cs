using HealthCare.Data.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Web.Models.Members
{
    public class MemberViewModel : HealthCare.Data.Models.Member
    {
        [Display(Name = "Reg No")]
        public string RegNoView
        {
            get { return RegNo.ToString().PadLeft(6, '0'); }
            set { value = RegNoView; }
        }
        public string RegName
        {
            get { return string.Format("{0}-{1}", RegNoView, Name); }
            set { value = RegNoView; }
        }
        public IFormFile FileData { get; set; }

        [EnumDataType(typeof(MemberType))]
        public MemberType MemberType { get; set; }
    }
    public enum MemberType
    {
        General = 1,
        Mainbody = 2
    }
}
