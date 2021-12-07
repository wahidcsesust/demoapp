using HealthCare.Data.Common;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using HealthCare.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class HelpCollectionController : Controller
    {
        private readonly IGenericRepository<HelpCollection> _helpCollectionRepository;
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ICompositeViewEngine _viewEngine;

        public HelpCollectionController(IGenericRepository<HelpCollection> helpCollectionRepository, IGenericRepository<Member> memberRepository, UserManager<User> userManager, IMemoryCache cache,
            IHostingEnvironment appEnvironment,
            ICompositeViewEngine viewEngine)
        {
            _helpCollectionRepository = helpCollectionRepository;
            _memberRepository = memberRepository;
            _userManager = userManager;
            _cache = cache;
            _appEnvironment = appEnvironment;
            _viewEngine = viewEngine;
        }
        public int GetNextSerialNumber()
        {
            int nextSerialNo = 1;
            if (_helpCollectionRepository.GetAllActive().Result.Any())
            {
                nextSerialNo = _helpCollectionRepository.GetAllActive().Result.ToList().Max(c => c.SerialNo) + 1;
            }
            return nextSerialNo;
        }
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var helpCollections = await _helpCollectionRepository.GetAllActive();

            var helpCollectionViewModelList = helpCollections.Select(m => new HelpCollectionViewModel()
            {
                Id = m.Id,
                SerialNo = m.SerialNo,
                Name = m.Name,
                Subject = m.Subject,
                DateOfHelpStr = m.DateOfHelpString,
                RefMemberName = m.Member.Name,
                Age = m.Age,
                Gender = m.Gender,
                MobileNumber = m.MobileNumber,
                Address = m.Address,
            }).ToList();

            return View(helpCollectionViewModelList);
        }
        [HttpGet]
        public async Task<PartialViewResult> CreateEdit(long id = 0, string type = null)
        {
            HelpCollectionViewModel model = new HelpCollectionViewModel()
            {
                DateOfHelpStr = DateTime.Now.ToString(Constants.DateFormat),
                Address = "Islampur",
            };
            var members = _memberRepository.GetAllActive().Result.OrderBy(r => r.RegNo).ToList();
            model.Members = members.Select(x => new SelectListItem
            {
                Text = x.Name + " (" +  x.RegNo.ToString().PadLeft(4, '0') + ")",
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var helpCollection = await _helpCollectionRepository.Get(id);
                if (helpCollection != null)
                {
                    model.Id = helpCollection.Id;
                    model.SerialNo = helpCollection.SerialNo;
                    model.Subject = helpCollection.Subject;
                    model.MemberId = helpCollection.MemberId;
                    model.Name = helpCollection.Name;
                    model.Age = helpCollection.Age;
                    model.Gender = helpCollection.Gender;
                    model.DateOfHelpStr = helpCollection.DateOfHelp != null ? helpCollection.DateOfHelp.Value.ToString(Constants.DateFormat) : "";
                    model.ReligionEnum = Utility.GetReligion(helpCollection.Religion);

                    model.MobileNumber = helpCollection.MobileNumber;
                    model.Address = helpCollection.Address;

                    model.FatherName = helpCollection.FatherName;
                    model.MotherName = helpCollection.MotherName;
                    model.ProfessionEnum = Utility.GetProfession(helpCollection.Profession);

                    model.NationalIdNo = helpCollection.NationalIdNo;
                }
            }

            return PartialView("EditorTemplates/_HelpCollection", model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(HelpCollectionViewModel model)
        {
            var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var member = await _helpCollectionRepository.Get(model.Id);
                    if (member == null)
                    {
                        member = new HelpCollection();
                        member.SerialNo = GetNextSerialNumber();
                    }

                    member.Subject = model.Subject;
                    member.Name = model.Name;
                    member.Age = model.Age;
                    member.Gender = model.Gender;
                    member.DateOfHelp = model.DateOfHelpStr.StringToDate();

                    member.MobileNumber = model.MobileNumber;
                    member.Address = model.Address;
                    member.MemberId = model.MemberId;


                    member.FatherName = model.FatherName;
                    member.Profession = model.ProfessionEnum.ToString();
                    member.MotherName = model.MotherName;
                    member.Religion = model.ReligionEnum.ToString();
                    member.NationalIdNo = model.NationalIdNo;

                    member.CreatedBy = user.Id;
                    member.ModifiedBy = user.Id;

                    await _helpCollectionRepository.Save(member);

                    return RedirectToAction("Manage");
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/_HelpCollection", model);
        }
        [HttpGet]
        public async Task<PartialViewResult> Delete(long id = 0)
        {
            ObjectDeleteViewModel model = new ObjectDeleteViewModel();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var member = await _helpCollectionRepository.Get(id);
                if (member != null)
                {
                    model.Id = member.Id;
                    model.Name = member.Name;
                }
            }

            return PartialView("DisplayTemplates/_DeleteHelpCollection", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var helpCollection = await _helpCollectionRepository.Get(model.Id);
                if (helpCollection != null)
                {
                    helpCollection.IsDeleted = true; ;
                    await _helpCollectionRepository.Update(helpCollection);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> PrintHelpCollection(long helpCollectionId = 0)
        {
            var helpCollection = await _helpCollectionRepository.Get(helpCollectionId);
            var renderedView = await RenderPartialViewToString("DisplayTemplates/_PrintHelpCollection", helpCollection);
            return Json(new { html = renderedView });
        }
        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
