using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Data.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        [ScaffoldColumn(false)]
        public bool IsNew { get { return Id <= 0; } }
        public virtual void CleanBeforeSave()
        {
            if (IsNew)
            {
                IsActive = true;
                IsDeleted = false;
                IsLocked = false;
                CreatedDate = DateTime.Now;
                ModifiedDate = DateTime.Now;
            }
        }
    }
}
