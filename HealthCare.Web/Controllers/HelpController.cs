using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Manage()
        {
            return View();
        }
    }
}
