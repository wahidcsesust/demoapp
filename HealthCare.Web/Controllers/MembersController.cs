using HealthCare.Data.Common;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using HealthCare.Web.Models.Collection;
using HealthCare.Web.Models.Members;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using HealthCare.Data.Interfaces;

namespace HealthCare.Web.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IGenericRepository<Collection> _collectionRepository;
        private readonly IGenericRepository<CashBackCollection> _cashBackRepository;
        private readonly ICompositeViewEngine _viewEngine;
        public MembersController(IMemberRepository memberRepository, UserManager<User> userManager, IMemoryCache cache,
            IHostingEnvironment appEnvironment, IGenericRepository<Collection> collectionRepository, IGenericRepository<CashBackCollection> cashBackRepository, ICompositeViewEngine viewEngine)
        {
            _memberRepository = memberRepository;
            _userManager = userManager;
            _cache = cache;
            _appEnvironment = appEnvironment;
            _collectionRepository = collectionRepository;
            _cashBackRepository = cashBackRepository;
            _viewEngine = viewEngine;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string id = null)
        {
            var objects = await _memberRepository.GetAll();

            //var members = objects.Where(p => !p.IsDeleted);
            var members = id == "2" ? objects.Where(p => !p.IsDeleted) : objects.Where(p => !p.IsDeleted);

            var memberViewModelList = members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                RegNo = m.RegNo,
                Name = m.Name,
                Gender = m.Gender,
                MobileNumber = m.MobileNumber,
                Address = m.Address,
                //Amount = m.Amount,
                FatherName = m.FatherName,
                //IsMainBody = m.IsMainBody,
                //TotalAmount = id == "2" ? _memberRepository.GetMainBodyCollectionById(m.Id) : m.TotalAmount,
                MemberType = id == "2" ? MemberType.Mainbody : MemberType.General
            }).ToList();
            return View(memberViewModelList.OrderBy(r => r.RegNo));
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateEdit(long id = 0, bool isMainBody = false)
        {
            MemberViewModel model = new MemberViewModel();

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
                    model.MobileNumber = member.MobileNumber;
                    model.Address = member.Address;
                    model.ImageName = member.ImageName;
                    model.ImageData = member.ImageData;
                    //model.Amount = member.Amount;
                    model.FatherName = member.FatherName;
                    //model.IsMainBody = member.IsMainBody;
                }
            }
            model.MemberType = isMainBody == true ? MemberType.Mainbody : MemberType.General;
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
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var member = await _memberRepository.Get(model.Id);

                    if (member == null)
                    {
                        member = new Member();
                        member.RegNo = GetNextRegNumber();
                    }
                    else
                    {
                        member.RegNo = model.RegNo;
                    }

                    member.Name = model.Name;
                    member.Age = model.Age;
                    member.Gender = model.Gender;
                    member.MobileNumber = model.MobileNumber;
                    member.Address = model.Address;
                    //member.Amount = model.Amount;
                    member.FatherName = model.FatherName;
                    //member.IsMainBody = model.IsMainBody;

                    member.CreatedBy = user.Id;
                    member.ModifiedBy = user.Id;

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
                                //var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                                var fileName = member.RegNo + Path.GetExtension(file.FileName);
                                if (System.IO.File.Exists(Path.Combine(uploads, fileName)))
                                {
                                    System.IO.File.Delete(Path.Combine(uploads, fileName));
                                }
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    member.ImageName = fileName;
                                }
                                //Save Image Data
                                using (var fs = file.OpenReadStream())
                                using (var ms = new MemoryStream())
                                {
                                    fs.CopyTo(ms);
                                    member.ImageData = ms.ToArray();
                                }
                            }
                        }
                    }

                    await _memberRepository.Save(member);
                    //return RedirectToAction("Manage");
                    return RedirectToAction("Manage", new { id = "1" });
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/_Member", model);
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

        [HttpGet]
        public async Task<PartialViewResult> ViewCollection(long id = 0, bool isMainBody = false)
        {
            var collections = await _collectionRepository.GetAllActive();

            var objects = isMainBody ? collections.Where(p => p.MemberId == id && p.IsMainBody) :
                                                collections.Where(p => p.MemberId == id && !p.IsMainBody);

            var list = new List<Models.Collection.CollectionViewModel>();

            list = objects.Select(a => new Models.Collection.CollectionViewModel()
            {
                Id = a.Id,
                Amount = a.Amount,
                CollectionDate = Utility.GetDateByValue(a.DateOfCollection),
                DateOfCollection = Utility.GetDateFormatByValue(a.DateOfCollection),
                IsMainbody = a.IsMainBody
            }).ToList();

            var member = await _memberRepository.Get(id);

            ViewData["RegNo"] = member != null ? member.RegNo.ToString().PadLeft(6, '0') : string.Empty;
            ViewData["Name"] = member != null ? member.Name : string.Empty;
            ViewData["TotalAmount"] = list.Select(a => a.Amount).Sum();

            var cashBack = await _cashBackRepository.GetAllActive();
            //ViewData["CashBackAmount"] = member.CashBackAmount ?? 0; //cashBack.Where(m => m.MemberId == id).Select(a => a.Amount).Sum();

            ViewData["IsMainBody"] = isMainBody ? 1 : 0;

            var finalList = list.OrderBy(x => x.CollectionDate.TimeOfDay).ThenBy(x => x.CollectionDate.Date).ThenBy(x => x.CollectionDate.Year);

            return PartialView("DisplayTemplates/_Collection", finalList);
        }

        [HttpGet]
        public async Task<JsonResult> PrintCollection(long id = 0)
        {
            var collections = await _collectionRepository.GetAll();

            var objects = collections.Where(p => p.MemberId == id && !p.IsDeleted);

            var list = new List<Models.Collection.CollectionViewModel>();

            list = objects.Select(a => new Models.Collection.CollectionViewModel()
            {
                Id = a.Id,
                Amount = a.Amount,
                CollectionDate = new DateTime(Convert.ToInt32(a.DateOfCollection.Split('/')[2]),
                Convert.ToInt32(a.DateOfCollection.Split('/')[0]), Convert.ToInt32(a.DateOfCollection.Split('/')[1])),
                DateOfCollection = string.Format("{0}-{1}-{2}", a.DateOfCollection.Split('/')[1],
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(a.DateOfCollection.Split('/')[0])), a.DateOfCollection.Split('/')[2]),
            }).ToList();
            var renderedView = await RenderPartialViewToString("DisplayTemplates/_CollectionPrint", list);
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<JsonResult> GetCollection(long id = 0)
        {
            var collections = await _collectionRepository.GetAll();

            var objects = collections.Where(p => p.MemberId == id && !p.IsDeleted);

            var list = new List<Models.Collection.CollectionViewModel>();

            list = objects.Select(a => new Models.Collection.CollectionViewModel()
            {
                Id = a.Id,
                Amount = a.Amount,
                CollectionDate = new DateTime(Convert.ToInt32(a.DateOfCollection.Split('/')[2]),
                Convert.ToInt32(a.DateOfCollection.Split('/')[0]), Convert.ToInt32(a.DateOfCollection.Split('/')[1])),
                DateOfCollection = string.Format("{0}-{1}-{2}", a.DateOfCollection.Split('/')[1],
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(a.DateOfCollection.Split('/')[0])), a.DateOfCollection.Split('/')[2]),
            }).ToList();

            var member = await _memberRepository.Get(id);

            ViewData["RegNo"] = member != null ? member.RegNo.ToString().PadLeft(6, '0') : string.Empty;
            ViewData["Name"] = member != null ? member.Name : string.Empty;
            ViewData["TotalAmount"] = list.Select(a => a.Amount).Sum();

            var finalList = list.OrderBy(x => x.CollectionDate.TimeOfDay).ThenBy(x => x.CollectionDate.Date).ThenBy(x => x.CollectionDate.Year);

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_Collection", finalList);

            return Json(new { html = renderedView });
            //return PartialView("DisplayTemplates/_Collection", finalList);
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

        public int GetNextRegNumber()
        {
            int nextRegNo = 1;
            if (_memberRepository.GetAll().Result.Where(c => !c.IsDeleted).Any())
            {
                nextRegNo = _memberRepository.GetAll().Result.ToList().Where(c => !c.IsDeleted).Max(c => c.RegNo) + 1;
            }
            return nextRegNo;
        }

        [HttpGet]
        public async Task<JsonResult> GetMemberTotalAmount(long memberId)
        {
            var member = await _memberRepository.Get(memberId);

            var amount = 0;//member.TotalAmount;

            return Json(new { amount = amount });
        }
    }
}
