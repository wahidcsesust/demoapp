using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using HealthCare.Web.Models.ProductGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class ProductGroupController : Controller
    {
        private readonly IGenericRepository<ProductGroup> _productGroupRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly UserManager<User> _userManager;

        public ProductGroupController(IGenericRepository<ProductGroup> productGroupRepository, IGenericRepository<Category> categoryRepository, UserManager<User> userManager)
        {
            _productGroupRepository = productGroupRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _productGroupRepository.GetAll();

            var productGroups = objects.Where(a => !a.IsDeleted);

            var productGroupsViewModelList = new List<ProductGroupViewModel>();
            foreach (var productGroup in productGroups)
            {
                var category = new Category();
                var productGroupViewModel = new ProductGroupViewModel();
                productGroupViewModel.Id = productGroup.Id;
                productGroupViewModel.Code = productGroup.Code;
                productGroupViewModel.Name = productGroup.Name;
                category = await _categoryRepository.Get(productGroup.CategoryId);
                productGroupViewModel.CategoryName = category != null ? category.Name : string.Empty;
                productGroupsViewModelList.Add(productGroupViewModel);
            }

            return View(productGroupsViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            ProductGroupViewModel model = new ProductGroupViewModel();
            var categories = await _categoryRepository.GetAll();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var productGroup = await _productGroupRepository.Get(id);
                if (productGroup != null)
                {
                    model.Code = productGroup.Code;
                    model.Name = productGroup.Name;
                    model.CategoryId = productGroup.CategoryId;
                }
            }
            return View("EditorTemplates/ProductGroup", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productGroup = await _productGroupRepository.Get(model.Id);
                    var user = await _userManager.GetUserAsync(User);
                    if (productGroup != null)
                    {
                        productGroup.Code = model.Code;
                        productGroup.Name = model.Name;
                        productGroup.CategoryId = model.CategoryId;
                        productGroup.IsActive = true;
                        productGroup.ModifiedDate = DateTime.Now;
                        productGroup.ModifiedBy = user.Id;
                        await _productGroupRepository.Update(productGroup);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        var aProductGroup = new ProductGroup();
                        aProductGroup.Code = model.Code;
                        aProductGroup.Name = model.Name;
                        aProductGroup.CategoryId = model.CategoryId;
                        aProductGroup.IsActive = true;
                        aProductGroup.CreatedDate = DateTime.Now;
                        aProductGroup.ModifiedDate = DateTime.Now;
                        aProductGroup.CreatedBy = user.Id;
                        aProductGroup.ModifiedBy = user.Id;
                        await _productGroupRepository.Insert(aProductGroup);
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

            // If we got this far, something failed, redisplay form
            var categories = await _categoryRepository.GetAll();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View("EditorTemplates/ProductGroup", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var productGroup = await _productGroupRepository.Get(id);
                if (productGroup != null)
                {
                    name = productGroup.Name;
                }
            }

            return PartialView("DisplayTemplates/_DeleteProductGroup", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var productGroup = await _productGroupRepository.Get(id);
                if (productGroup != null)
                {
                    //var result = _productGroupRepository.Delete(productGroup);
                    productGroup.IsDeleted = true; ;
                    await _productGroupRepository.Update(productGroup);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}