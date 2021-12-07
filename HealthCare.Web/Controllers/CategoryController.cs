using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly UserManager<User> _userManager;
        public CategoryController(IGenericRepository<Category> categoryRepository, UserManager<User> userManager)
        {
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _categoryRepository.GetAll();
            var categories = objects.Where(c => !c.IsDeleted);

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id=0)
        {
            var category = new Category();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                category = await _categoryRepository.Get(id);
            }
            return View("EditorTemplates/Category", category);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Category model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var category = await _categoryRepository.Get(model.Id);
                    if (category != null)
                    {
                        category.Code = model.Code;
                        category.Name = model.Name;
                        category.IsActive = true;
                        category.ModifiedDate = DateTime.Now;
                        category.ModifiedBy = user.Id;
                        await _categoryRepository.Update(category);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        category = model;
                        category.IsActive = true;
                        category.CreatedDate = DateTime.Now;
                        category.CreatedBy = user.Id;
                        category.ModifiedDate = DateTime.Now;
                        category.ModifiedBy = user.Id;

                        await _categoryRepository.Insert(category);
                        return RedirectToAction("Manage");
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View("EditorTemplates/Category", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var category = await _categoryRepository.Get(id);
                if (category != null)
                {
                    name = category.Name;
                }
            }
            return PartialView("DisplayTemplates/_DeleteCategory", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var category = await _categoryRepository.Get(id);
                if (category != null)
                {
                    //var result = _categoryRepository.Delete(category);
                    category.IsDeleted = true; ;
                    await _categoryRepository.Update(category);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}