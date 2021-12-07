using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models.Modal
{
    public class BootstrapModel
    {
        public string ID { get; set; }
        public string AreaLabeledId { get; set; }
        public ModalSize Size { get; set; }
        public string Message { get; set; }
        public string ModalSizeClass
        {
            get
            {
                switch (this.Size)
                {
                    case ModalSize.Small:
                        return "modal-sm-custom";
                    case ModalSize.Large:
                        return "modal-lg-custom";
                    case ModalSize.Medium:
                        return "modal-md-custom";
                    default:
                        return "";
                }
            }
        }
    }
}
