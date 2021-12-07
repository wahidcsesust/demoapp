using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using HealthCare.Web.Models.TestCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class TestCategoryController : Controller
    {
        private readonly IGenericRepository<TestCategory> _testCategoryRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly UserManager<User> _userManager;
        public TestCategoryController(IGenericRepository<TestCategory> testCategoryRepository, IGenericRepository<Doctor> doctorRepository, UserManager<User> userManager)
        {
            _testCategoryRepository = testCategoryRepository;
            _doctorRepository = doctorRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            List<TestCategoryViewModel> models = new List<TestCategoryViewModel>();
            var departments = await _testCategoryRepository.GetAllActive();

            models = departments.Select(d => new TestCategoryViewModel
            {
                Id = d.Id,
                Name = d.Name,
                NoOfMedicalTests = d.MedicalTests.LongCount()
        }).ToList();

            return View("Manage",models);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            TestCategoryViewModel model = new TestCategoryViewModel();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var department = await _testCategoryRepository.Get(id);
                if (department != null)
                {
                    model.Name = department.Name;
                }
            }
            return PartialView("EditorTemplates/_TestCategory", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(TestCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var testCategory = await _testCategoryRepository.Get(model.Id);
                    if (testCategory != null)
                    {
                        testCategory.Name = model.Name;
                        testCategory.IsActive = true;
                        testCategory.ModifiedDate = DateTime.Now;
                        testCategory.ModifiedBy = user.Id;
                        await _testCategoryRepository.Update(testCategory);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        TestCategory aTestCategory = new TestCategory();
                        aTestCategory.Name = model.Name;
                        aTestCategory.IsActive = true;
                        aTestCategory.CreatedDate = DateTime.Now;
                        aTestCategory.CreatedBy = user.Id;
                        aTestCategory.ModifiedDate = DateTime.Now;
                        aTestCategory.ModifiedBy = user.Id;

                        await _testCategoryRepository.Insert(aTestCategory);
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
            return View("EditorTemplates/TestCategory", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var department = await _testCategoryRepository.Get(id);
                if (department != null)
                {
                    name = department.Name;
                }
            }
            return PartialView("DisplayTemplates/_DeleteTestCategory", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var testCategory = await _testCategoryRepository.Get(model.Id);
                if (testCategory != null)
                {
                    //var result = _departmentRepository.Delete(department);
                    testCategory.IsDeleted = true; ;
                    await _testCategoryRepository.Update(testCategory);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}