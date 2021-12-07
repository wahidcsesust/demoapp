using HealthCare.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class SmsViewModel
    {
        [EnumDataType(typeof(MemberCategoryEnum))]
        public MemberCategoryEnum MemberCategoryEnum { get; set; }
        public string Message { get; set; }
        public long MemberId { get; set; }
        public string SingleMessage { get; set; }
        public IEnumerable<SelectListItem> Members { get; set; }
    }
}
