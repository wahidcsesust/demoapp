using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models.Collection
{
    public class CollectionViewModel
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public decimal TempAmount { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DateOfCollection { get; set; }
        public long MemberId { get; set; }
        public string MemberName { get; set; }
        public List<SelectListItem> Members { get; set; }
        public string CollectionTypeId { get; set; }
        public bool IsMainbody { get; set; }
        public string CollectionFormHeading { get; set; }
        public long DayId { get; set; }
        public List<SelectListItem> Days { get; set; }
    }
}
