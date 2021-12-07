using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Authorization;
using HealthCare.Web.Models.ProductBrand;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class ProductBrandController : Controller
    {
        private readonly IGenericRepository<ProductGroup> _productGroupRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly UserManager<User> _userManager;

        public ProductBrandController(IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductGroup> productGroupRepository, IGenericRepository<Category> categoryRepository, UserManager<User> userManager)
        {
            _productBrandRepository = productBrandRepository;
            _productGroupRepository = productGroupRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _productBrandRepository.GetAll();

            var productBrands = objects.Where(a => !a.IsDeleted);

            var productBrandViewModelList = new List<ProductBrandViewModel>();

            foreach (var productBrand in productBrands)
            {
                var category = new Category();
                var productGroup = new ProductGroup();
                var productBrandViewModel = new ProductBrandViewModel();
                productBrandViewModel.Id = productBrand.Id;
                productBrandViewModel.Code = productBrand.Code;
                productBrandViewModel.Name = productBrand.Name;

                category = await _categoryRepository.Get(productBrand.CategoryId);
                productBrandViewModel.CategoryName = category != null ? category.Name : string.Empty;

                productGroup = await _productGroupRepository.Get(productBrand.ProductGroupId);
                productBrandViewModel.ProductGroupName = productGroup != null ? productGroup.Name : string.Empty;

                productBrandViewModelList.Add(productBrandViewModel);
            }
            return View(productBrandViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            ProductBrandViewModel model = new ProductBrandViewModel();
            var categories = await _categoryRepository.GetAll();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var productGroups = await _productGroupRepository.GetAll();
            model.ProductGroups = productGroups.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var productBrand = await _productBrandRepository.Get(id);
                if (productBrand != null)
                {
                    model.Code = productBrand.Code;
                    model.Name = productBrand.Name;
                    model.CategoryId = productBrand.CategoryId;
                    model.ProductGroupId = productBrand.ProductGroupId;
                }
            }

            return View("EditorTemplates/ProductBrand", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(ProductBrandViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productBrand = await _productBrandRepository.Get(model.Id);
                    var user = await _userManager.GetUserAsync(User);
                    if (productBrand != null)
                    {
                        productBrand.Code = model.Code;
                        productBrand.Name = model.Name;
                        productBrand.ProductGroupId = model.ProductGroupId;
                        productBrand.CategoryId = model.CategoryId;
                        productBrand.IsActive = true;
                        productBrand.ModifiedDate = DateTime.Now;
                        productBrand.ModifiedBy = user.Id;
                        await _productBrandRepository.Update(productBrand);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        var aProductBrand = new ProductBrand();
                        aProductBrand.Code = model.Code;
                        aProductBrand.Name = model.Name;
                        aProductBrand.CategoryId = model.CategoryId;
                        aProductBrand.ProductGroupId = model.ProductGroupId;
                        aProductBrand.IsActive = true;
                        aProductBrand.CreatedDate = DateTime.Now;
                        aProductBrand.ModifiedDate = DateTime.Now;
                        aProductBrand.CreatedBy = user.Id;
                        aProductBrand.ModifiedBy = user.Id;
                        await _productBrandRepository.Insert(aProductBrand);
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
            var categories = await _categoryRepository.GetAll();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var productGroups = await _productGroupRepository.GetAll();
            model.ProductGroups = productGroups.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View("EditorTemplates/ProductBrand", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var productBrand = await _productBrandRepository.Get(id);
                if (productBrand != null)
                {
                    name = productBrand.Name;
                }
            }

            return PartialView("DisplayTemplates/_DeleteProductBrand", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var productBrand = await _productBrandRepository.Get(id);
                if (productBrand != null)
                {
                    //var result = _productBrandRepository.Delete(productBrand);
                    productBrand.IsDeleted = true; ;
                    await _productBrandRepository.Update(productBrand);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}