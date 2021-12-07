using HealthCare.Data.Common;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using HealthCare.Web.Models.Member;
using HealthCare.Web.Models.Report;
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
    public class MemberController : Controller
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ISmsService _smsService;

        public MemberController(IGenericRepository<Member> memberRepository, UserManager<User> userManager, IMemoryCache cache,
            IHostingEnvironment appEnvironment,
            ICompositeViewEngine viewEngine,
            ISmsService smsService)
        {
            _memberRepository = memberRepository;
            _userManager = userManager;
            _cache = cache;
            _appEnvironment = appEnvironment;
            _viewEngine = viewEngine;
            _smsService = smsService;
        }
        [HttpGet]
        public async Task<IActionResult> IdCard()
        {
            var member = await _memberRepository.Get(1);

            return View(member);
        }
        [HttpGet]
        public async Task<IActionResult> IdCardFinal()
        {
            var member = await _memberRepository.Get(1);

            return View(member);
        }
        [HttpGet]
        public async Task<IActionResult> Manage(string id = null)
        {
            var patients = await _memberRepository.GetAllActive();

            var category = "All";
            if (id == "1")
            {
                category = "AdvisoryCouncil";
            }
            else if (id == "2")
            {
                category = "ExecutiveCouncil";
            }
            else if (id == "3")
            {
                category = "GeneralMember";
            }
            else if (id == "4")
            {
                category = "Other";
            }

            var patientFilters = patients.Where(m => m.Category == category || category == "All").ToList();

            var patientViewModelList = patientFilters.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                RegNo = m.RegNo,
                Name = m.Name,
                FatherName = m.FatherName,
                Age = m.Age,
                Gender = m.Gender,
                BloodGroup = m.BloodGroupString,
                MobileNumber = m.MobileNumber,
                Address = m.Address,
                Designation = m.DesignationString,
                ImageName = m.ImageName
            }).ToList();

            return View(patientViewModelList.OrderBy(r => r.RegNo));
        }
        [HttpGet]
        public async Task<PartialViewResult> CreateEdit(long id = 0, string type = null)
        {
            MemberViewModel model = new MemberViewModel()
            {
                DateOfBirthStr = DateTime.Now.ToString(Constants.DateFormat),
                Nationality = "Bangladeshi",
                PassportNo = "NONE",
                Address = "Islampur",
                MemberDesignationEnum = MemberDesignationEnum.Member,
                MemberCategoryEnum = MemberCategoryEnum.GeneralMember
            };
            model.ModalType = type;

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var member = await _memberRepository.Get(id);
                if (member != null)
                {
                    model.Id = member.Id;
                    model.RegNo = member.RegNo;
                    model.Name = member.Name;
                    model.Age = member.Age;
                    model.Gender = member.Gender;
                    model.DateOfBirthStr = member.DateOfBirth != null ? member.DateOfBirth.Value.ToString(Constants.DateFormat) : "";
                    model.BloodGroupEnum = Utility.GetBloodGroup(member.BloodGroup);
                    model.AreaLocationEnum = Utility.GetAreaLocation(member.AreaLocation);

                    model.MemberCategoryEnum = Utility.GetMemberCategory(member.Category);
                    model.MemberDesignationEnum = Utility.GetMemberDesignation(member.Designation);
                    model.ReligionEnum = Utility.GetReligion(member.Religion);

                    model.MobileNumber = member.MobileNumber;
                    model.Address = member.Address;
                    model.ImageName = member.ImageName;
                    //model.ImageData = member.ImageData;


                    model.FatherName = member.FatherName;
                    model.ProfessionEnum = Utility.GetProfession(member.Profession);

                    model.MotherName = member.MotherName;

                    model.Nationality = member.Nationality;

                    model.NationalIdNo = member.NationalIdNo;
                    model.PassportNo = member.PassportNo;

                    model.NomineeName = member.NomineeName;
                    model.NomineeRelation = member.NomineeRelation;
                    model.NomineeNationalIdNo = member.NomineeNationalIdNo;
                    model.NomineeAge = member.NomineeAge;
                }
            }

            return PartialView("EditorTemplates/_Member", model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(MemberViewModel model)
        {
            var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            if (ModelState.IsValid)
            {
                bool isNew = false;
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var member = await _memberRepository.Get(model.Id);
                    if (member == null)
                    {
                        member = new Member();
                        member.RegNo = model.MemberCategoryEnum.ToString() == "AdvisoryCouncil" ? GetNextAdvisorRegNumber() : GetNextRegNumber();
                        isNew = true;
                    }
                    else
                    {
                        member.RegNo = model.RegNo;
                    }

                    member.Name = model.Name;
                    member.Age = model.Age;
                    member.Gender = model.Gender;
                    member.DateOfBirth = model.DateOfBirthStr.StringToDate();
                    member.BloodGroup = model.BloodGroupEnum.ToString();
                    member.AreaLocation = model.AreaLocationEnum.ToString();

                    member.Category = model.MemberCategoryEnum.ToString();
                    member.Designation = model.MemberDesignationEnum.ToString();

                    member.MobileNumber = model.MobileNumber;
                    member.Address = model.Address;


                    member.FatherName = model.FatherName;
                    member.Profession = model.ProfessionEnum.ToString();
                    member.MotherName = model.MotherName;
                    member.Nationality = model.Nationality;
                    member.Religion = model.ReligionEnum.ToString();
                    member.NationalIdNo = model.NationalIdNo;
                    member.PassportNo = model.PassportNo;

                    member.NomineeName = model.NomineeName;
                    member.NomineeRelation = model.NomineeRelation;

                    member.NomineeNationalIdNo = model.NomineeNationalIdNo;
                    member.NomineeAge = model.NomineeAge;

                    member.CreatedBy = user.Id;
                    member.ModifiedBy = user.Id;

                    await _memberRepository.Save(member);

                    //Upload Image
                    var files = HttpContext.Request.Form.Files;

                    foreach (var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {

                            var file = Image;
                            //There is an error here
                            var uploads = Path.Combine(_appEnvironment.WebRootPath, "uploads\\img");

                            if (file.Length > 0)
                            {
                                var fileName = member.Id + Path.GetExtension(file.FileName);
                                if (System.IO.File.Exists(Path.Combine(uploads, fileName)))
                                {
                                    System.IO.File.Delete(Path.Combine(uploads, fileName));
                                }
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    member.ImageName = fileName;
                                }
                            }
                        }
                    }
                    await _memberRepository.Save(member);

                    // Send SMS for New Member
                    if (isNew)
                    {
                        var newMember = await _memberRepository.Get(member.Id);
                        var mobileNumber = (newMember != null && !string.IsNullOrEmpty(newMember.MobileNumber)) ? newMember.MobileNumber : "01719012103";
                        var message = $"অভিনন্দন {member.Name}, সংগঠনে আপনাকে স্বাগতম. আপনার আইডি নং : {member.RegNo.ToString().PadLeft(5, '0')}";
                        if (_smsService.SendSms(mobileNumber, message))
                        {
                            isNew = false;
                        }
                    }

                    //return RedirectToAction("Manage");
                    var idRedirect = "0";
                    if (member.Category == MemberCategoryEnum.AdvisoryCouncil.ToString())
                        idRedirect = "1";
                    else if (member.Category == MemberCategoryEnum.ExecutiveCouncil.ToString())
                        idRedirect = "2";
                    else if (member.Category == MemberCategoryEnum.GeneralMember.ToString())
                        idRedirect = "3";
                    else if (member.Category == MemberCategoryEnum.Other.ToString())
                        idRedirect = "4";
                    return RedirectToAction("Manage", new { id = idRedirect });
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/_Members", model);
        }
        [HttpGet]
        public async Task<JsonResult> PrintMember(long memberId = 0)
        {
            var member = await _memberRepository.Get(memberId);

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_MemberPrint", member);
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<JsonResult> PrintIdCard(long memberId = 0)
        {
            var member = await _memberRepository.Get(memberId);

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_IdCardFinal", member);
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<JsonResult> PrintMemberForm(long memberId = 0)
        {
            var renderedView = await RenderPartialViewToString("DisplayTemplates/_MemberPrintForm", new Member());
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<JsonResult> PrintCouncilForm(long memberId = 0)
        {
            var renderedView = await RenderPartialViewToString("DisplayTemplates/_CouncilorPrintForm", new Member());
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<JsonResult> PrintHelpForm(long memberId = 0)
        {
            var renderedView = await RenderPartialViewToString("DisplayTemplates/_HelpForm", new Member());
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<PartialViewResult> Delete(long id = 0)
        {
            ObjectDeleteViewModel model = new ObjectDeleteViewModel();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var member = await _memberRepository.Get(id);
                if (member != null)
                {
                    model.Id = member.Id;
                    model.Name = member.Name;
                }
            }

            return PartialView("DisplayTemplates/_DeleteMember", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var member = await _memberRepository.Get(model.Id);
                if (member != null)
                {
                    member.IsDeleted = true; ;
                    await _memberRepository.Update(member);
                    //return RedirectToAction("Manage");
                    return RedirectToAction("Manage", new { id = "1" });
                }
            }
            return View();
        }
        public int GetNextRegNumber()
        {
            int nextRegNo = 1;
            if (_memberRepository.GetAllActive().Result.Where(m => m.Category == "GeneralMember").Any())
            {
                nextRegNo = _memberRepository.GetAllActive().Result.Where(m => m.Category == "GeneralMember").ToList().Max(c => c.RegNo) + 1;
            }
            return nextRegNo;
        }
        public int GetNextAdvisorRegNumber()
        {
            int nextRegNo = 501;
            if (_memberRepository.GetAllActive().Result.Where(m => m.Category == "AdvisoryCouncil").Any())
            {
                nextRegNo = _memberRepository.GetAllActive().Result.Where(m => m.Category == "AdvisoryCouncil").ToList().Max(c => c.RegNo) + 1;
            }
            return nextRegNo;
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
