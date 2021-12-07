using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using HealthCare.Web.Models.MedicalTest;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class MedicalTestController : Controller
    {
        private readonly IGenericRepository<MedicalTest> _medicalTestRepository;
        private readonly IGenericRepository<TestCategory> _testCategoryRepository;
        private readonly UserManager<User> _userManager;
        public MedicalTestController(IGenericRepository<MedicalTest> medicalTestRepository, IGenericRepository<TestCategory> testCategoryRepository, UserManager<User> userManager)
        {
            _medicalTestRepository = medicalTestRepository;
            _testCategoryRepository = testCategoryRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _medicalTestRepository.GetAll();
            var medicalTests = objects.Where(r => r.IsDeleted == false);

            var medicalTestViewModelList = new List<MedicalTestViewModel>();

            foreach(var medicalTest in medicalTests)
            {
                var medicalTestViewModel = new MedicalTestViewModel();

                medicalTestViewModel.Id = medicalTest.Id;
                medicalTestViewModel.Code = medicalTest.Code;
                medicalTestViewModel.Name = medicalTest.Name;
                medicalTestViewModel.Amount = medicalTest.Amount ?? 0;
                medicalTestViewModel.TestCategoryName = medicalTest.TestCategory.Name;
                medicalTestViewModelList.Add(medicalTestViewModel);
            }

            return View(medicalTestViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            var model = new MedicalTestViewModel();

            var testCategories = await _testCategoryRepository.GetAllActive();
            model.TestCategories = testCategories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var medicaltest = await _medicalTestRepository.Get(id);
                if (medicaltest != null)
                {
                    model.Code = medicaltest.Code;
                    model.Name = medicaltest.Name;
                    model.Amount = medicaltest.Amount ?? 0;
                    model.TestCategoryId = medicaltest.TestCategoryId;
                }
            }
            return PartialView("EditorTemplates/_MedicalTest", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(MedicalTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var medicalTest = await _medicalTestRepository.Get(model.Id);
                    if (medicalTest != null)
                    {
                        medicalTest.Code = model.Code;
                        medicalTest.Name = model.Name;
                        medicalTest.Amount = model.Amount;
                        medicalTest.TestCategoryId = model.TestCategoryId;

                        medicalTest.IsActive = true;
                        medicalTest.ModifiedDate = DateTime.Now;
                        medicalTest.ModifiedBy = user.Id;
                        await _medicalTestRepository.Update(medicalTest);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        var aMedicalTest = new MedicalTest();
                        aMedicalTest.Code = model.Code;
                        aMedicalTest.Name = model.Name;
                        aMedicalTest.Amount = model.Amount;
                        aMedicalTest.TestCategoryId = model.TestCategoryId;

                        aMedicalTest.IsActive = true;
                        aMedicalTest.CreatedDate = DateTime.Now;
                        aMedicalTest.CreatedBy = user.Id;
                        aMedicalTest.ModifiedDate = DateTime.Now;
                        aMedicalTest.ModifiedBy = user.Id;

                        await _medicalTestRepository.Insert(aMedicalTest);
                        return RedirectToAction("Manage");
                    }
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }
            var testCategories = await _testCategoryRepository.GetAll();
            model.TestCategories = testCategories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View("EditorTemplates/MedicalTest", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var medicalTest = await _medicalTestRepository.Get(id);
                if (medicalTest != null)
                {
                    name = medicalTest.Name;
                }
            }
            return PartialView("DisplayTemplates/_DeleteMedicalTest", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var medicalTest = await _medicalTestRepository.Get(model.Id);
                if (medicalTest != null)
                {
                    medicalTest.IsDeleted = true; ;
                    await _medicalTestRepository.Update(medicalTest);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}