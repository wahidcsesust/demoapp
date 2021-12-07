using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Data.Models
{
    public class PatientTestDetail : BaseEntity
    {
        public long PatientTestId { get; set; }
        public virtual PatientTest PatientTest { get; set; }
        public long MedicalTestId { get; set; }
        public virtual MedicalTest MedicalTest { get; set; }
        public decimal? TestRate { get; set; }
        public int? Discount { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> MedicalTests { get; set; }
        [NotMapped]
        private Guid _guid;
        [ScaffoldColumn(false)]
        [NotMapped]
        public virtual Guid Guid
        {
            get { return _guid = (_guid == Guid.Empty ? Guid.NewGuid() : _guid); }
            set { _guid = value; }
        }
    }
}
