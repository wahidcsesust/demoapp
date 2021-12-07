using Microsoft.AspNetCore.Mvc.Rendering;
using HealthCare.Data.Models;
using System.Collections.Generic;

namespace HealthCare.Web.Models.Accounting
{
    public class AccountHeadViewModel : AccountHead
    {
        public string AccountHeadName { get; set; }
        public string AccountHeadTypeName { get; set; }
        public List<SelectListItem> AcconuntHeadTypes { get; set; }
    }
}
