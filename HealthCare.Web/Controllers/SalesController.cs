using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Web.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Manage()
        {
            return View();
        }
    }
}