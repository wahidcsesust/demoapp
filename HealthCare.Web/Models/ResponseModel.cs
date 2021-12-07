using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCare.Web.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
		public string Details { get; set; }
		public Object Data { get; set; }
    }
}
