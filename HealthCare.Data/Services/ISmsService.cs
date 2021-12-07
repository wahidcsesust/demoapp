using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public interface ISmsService
    {
        bool SendSms(string mobileNumbers, string message);
    }
}
