using HealthCare.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class HelpCollectionViewModel : HelpCollection
    {
        public IEnumerable<SelectListItem> Members { get; set; }
        public string DateOfHelpStr { get; set; }
        [EnumDataType(typeof(ProfessionEnum))]
        public ProfessionEnum ProfessionEnum { get; set; }

        [EnumDataType(typeof(ReligionEnum))]
        public ReligionEnum ReligionEnum { get; set; }
        public string RefMemberName { get; set; }
    }
}
